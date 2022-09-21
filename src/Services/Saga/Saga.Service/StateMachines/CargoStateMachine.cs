using Core.Domain;
using Core.Domain.Enums;
using Cargos;
using MassTransit;
using Saga.Application.Cargos;
using Saga.Domain.Instances;
using Routes;
using Saga.Application.Routes;
using Deliveries;
using Saga.Application.Deliveries;
using Saga.Application.Payments;
using Payments;

namespace Saga.Service.StateMachines;

public class CargoStateMachine : MassTransitStateMachine<CargoStateInstance>
{
    #region States

    public State CreateCargo { get; set; }
    public State SendSelfie { get; set; }
    public State CargoApproval { get; set; }
    public State CargoRejected { get; set; }

    public State StartRoute { get; set; }
    public State AutoRoute { get; set; }
    public State ManuelRoute { get; set; }

    // Delivery
    public State StartDelivery { get; set; }
    public State NewDelivery { get; set; }
    public State NotDelivered { get; set; }
    public State CreateRefund { get; set; }
    public State CreateDelivery { get; set; }

    // Payment
    public State CardPayment { get; set; }
    public State FreeDelivery { get; set; }
    public State PayAtDoor { get; set; }

    public State DeliveryCompleted { get; set; }
    public State ShiftCompletion { get; set; }

    #endregion

    #region Events

    public Event<ICreateCargo> CreateCargoEvent { get; private set; }
    public Event<ISendSelfie> SendSelfieEvent { get; private set; }
    public Event<ICargoApproval> CargoApprovalEvent { get; private set; }
    public Event<ICargoRejected> CargoRejectedEvent { get; private set; }

    public Event<IStartRoute> StartRouteEvent { get; private set; }
    public Event<IAutoRoute> AutoRouteEvent { get; private set; }
    public Event<IManuelRoute> ManuelRouteEvent { get; private set; }

    public Event<IStartDelivery> StartDeliveryEvent { get; private set; }
    public Event<INewDelivery> NewDeliveryEvent { get; private set; }
    public Event<ICreateDelivery> CreateDeliveryEvent { get; private set; }
    public Event<ICardPayment> CardPaymentEvent { get; private set; }
    public Event<IFreeDelivery> FreeDeliveryEvent { get; private set; }
    public Event<IPayAtDoor> PayAtDoorEvent { get; private set; }

    public Event<INotDelivered> NotDeliveredEvent { get; private set; }
    public Event<ICreateRefund> CreateRefundEvent { get; private set; }

    public Event<IDeliveryCompleted> DeliveryCompletedEvent { get; private set; }
    public Event<IShiftCompletion> ShiftCompletionEvent { get; private set; }

    #endregion

    [Obsolete]
    public CargoStateMachine()
    {
        QueueConfigurationExtensions.AddQueueConfiguration(null, out IQueueConfiguration queueConfiguration);
        InstanceState(instance => instance.CurrentState);

        #region Event

        Event(() => CreateCargoEvent, instance => instance.CorrelateBy<Guid>(state => state.UserId, context => context.Message.DebitId).SelectId(s => Guid.NewGuid()));
        Event(() => SendSelfieEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CargoApprovalEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CargoRejectedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => StartRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => AutoRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => ManuelRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => StartDeliveryEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => NewDeliveryEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CardPaymentEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => FreeDeliveryEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => PayAtDoorEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CreateDeliveryEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => NotDeliveredEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CreateRefundEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => DeliveryCompletedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => ShiftCompletionEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        #endregion

        Initially(
            When(CreateCargoEvent)
                .Then(context =>
                {
                    context.Instance.UserId = context.Data.CourierId;
                    context.Instance.CreatedOn = DateTime.Now;
                })
                .TransitionTo(CreateCargo)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateCargo]}"), context => new CreateCargoCommand(context.Instance.CorrelationId)
                {
                    DebitId = context.Data.DebitId,
                    CourierId = context.Data.CourierId,
                    Cargos = context.Data.Cargos,
                    CurrentState = context.Instance.CurrentState
                }));

        #region Cargo

        During(CreateCargo,
             When(SendSelfieEvent)
                 .TransitionTo(SendSelfie)
                 .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.SendSelfie]}"), context => new SendSelfieCommand(context.Data.CorrelationId)
                 {
                     CorrelationId = context.Instance.CorrelationId,
                     CurrentState = context.Instance.CurrentState
                 }));

        During(SendSelfie,
         When(SendSelfieEvent)
             .TransitionTo(SendSelfie)
             .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.SendSelfie]}"), context => new SendSelfieCommand(context.Data.CorrelationId)
             {
                 CorrelationId = context.Instance.CorrelationId,
                 CurrentState = context.Instance.CurrentState
             }));

        During(SendSelfie,
           When(CargoApprovalEvent)
               .TransitionTo(CargoApproval)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoApproval]}"), context => new CargoApprovalCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
                   
               }));

        During(CargoApproval,
            When(StartRouteEvent)
               //.Then(context =>
               //{
               //    context.Instance.CargoRoutes = context.Data.CargoRoutes;
               //})
               .TransitionTo(StartRoute)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartRoute]}"), context => new StartRouteCommand(context.Data.CorrelationId)
               {
                    CurrentState = context.Instance.CurrentState,
                    CorrelationId = context.Instance.CorrelationId
               }),
            When(CargoRejectedEvent)
                .TransitionTo(CargoRejected)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoRejected]}"), context => new CargoRejectedCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId,
                    CurrentState = context.Instance.CurrentState
                }));

        #endregion

        #region Start Route

        During(StartRoute,
            When(AutoRouteEvent)
                .TransitionTo(AutoRoute)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.AutoRoute]}"), context => new AutoRouteCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId,
                    CurrentState = context.Instance.CurrentState

                }),
            When(ManuelRouteEvent)
                .TransitionTo(ManuelRoute)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.ManuelRoute]}"), context => new ManuelRouteCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId,
                    CurrentState = context.Instance.CurrentState
                })
            );

        During(AutoRoute,
         When(StartDeliveryEvent)
             .TransitionTo(StartDelivery)
             .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartDelivery]}"), context => new StartDeliveryCommand(context.Data.CorrelationId)
             {
                 CorrelationId = context.Instance.CorrelationId,
                 CurrentState = context.Instance.CurrentState
             }));

        During(ManuelRoute,
         When(StartDeliveryEvent)
             .TransitionTo(StartDelivery)
             .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartDelivery]}"), context => new StartDeliveryCommand(context.Data.CorrelationId)
             {
                 CorrelationId = context.Instance.CorrelationId,
                 CurrentState = context.Instance.CurrentState,
             }));

        #endregion

        #region Delivery

        #region Start Delivery


        During(StartDelivery,
             When(NewDeliveryEvent)
                 .TransitionTo(NewDelivery)
                 .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.NewDelivery]}"), context => new NewDeliveryCommand(context.Data.CorrelationId)
                 {
                     CorrelationId = context.Instance.CorrelationId,
                     CurrentState = context.Instance.CurrentState
                 }));

        During(NewDelivery,
           When(CreateDeliveryEvent)
               .TransitionTo(CreateDelivery)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateDelivery]}"), context => new CreateDeliveryCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
                   PaymentType = context.Instance.PaymentType
               }),
           When(NotDeliveredEvent)
               .TransitionTo(NotDelivered)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.NotDelivered]}"), context => new NotDeliveredCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
               }),
           When(CreateRefundEvent)
               .TransitionTo(CreateRefund)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateRefund]}"), context => new CreateRefundCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
               })
           );

        #endregion

        #region CreateDelivery

        During(CreateDelivery,
          When(CardPaymentEvent)
              .TransitionTo(CardPayment)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CardPayment]}"), context => new CardPaymentCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
                   PaymentType = context.Data.PaymentType
               }),
           When(FreeDeliveryEvent)
              .TransitionTo(FreeDelivery)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.FreeDelivery]}"), context => new FreeDeliveryCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
                   PaymentType = context.Data.PaymentType
               }),
            When(PayAtDoorEvent)
              .TransitionTo(PayAtDoor)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.PayAtDoor]}"), context => new PayAtDoorCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
                   PaymentType = context.Data.PaymentType
               })
          );

        During(CardPayment,
         When(DeliveryCompletedEvent)
             .TransitionTo(DeliveryCompleted)
              .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
              {
                  CorrelationId = context.Instance.CorrelationId,
                  CurrentState = context.Instance.CurrentState,
              })
         );

        During(FreeDelivery,
         When(DeliveryCompletedEvent)
             .TransitionTo(DeliveryCompleted)
              .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
              {
                  CorrelationId = context.Instance.CorrelationId,
                  CurrentState = context.Instance.CurrentState,
              })
         );

        During(PayAtDoor,
         When(DeliveryCompletedEvent)
             .TransitionTo(DeliveryCompleted)
              .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
              {
                  CorrelationId = context.Instance.CorrelationId,
                  CurrentState = context.Instance.CurrentState,
              })
         );
        #endregion

        #region NotDelivered

        During(NotDelivered,
          When(DeliveryCompletedEvent)
              .TransitionTo(DeliveryCompleted)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
               })
          );

        #endregion

        #region CreateRefund

        During(CreateRefund,
           When(DeliveryCompletedEvent)
               .TransitionTo(DeliveryCompleted)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId,
                    CurrentState = context.Instance.CurrentState,
                })
           );

        #endregion

        During(DeliveryCompleted,
           When(NewDeliveryEvent)
               .TransitionTo(NewDelivery)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.NewDelivery]}"), context => new NewDeliveryCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId,
                    CurrentState = context.Instance.CurrentState,
                })
           );

        During(DeliveryCompleted,
          When(ShiftCompletionEvent)
              .TransitionTo(ShiftCompletion)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.ShiftCompletion]}"), context => new ShiftCompletionCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
               }).Finalize()
          );

        #endregion

        SetCompletedWhenFinalized();
    }
}
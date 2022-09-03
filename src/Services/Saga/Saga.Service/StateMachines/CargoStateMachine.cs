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

namespace Saga.Service.StateMachines;

public class CargoStateMachine : MassTransitStateMachine<CargoStateInstance>
{
    public State CreateCargo { get; set; }
    public State SendSelfie { get; set; }
    public State CargoApproved { get; set; }
    public State CargoRejected { get; set; }

    public State StartRoute { get; set; }
    public State RouteConfirmed { get; set; }
    public State AutoRoute { get; set; }
    public State ManuelRoute { get; set; }

    // Delivery
    public State StartDelivery { get; set; }
    public State CreateDelivery { get; set; }
    public State CardPayment { get; set; }
    public State FreeDelivery { get; set; }
    public State PayAtDoor { get; set; }

    public State NotDelivered { get; set; }
    public State CreateRefund { get; set; }
    public State DeliveryCompleted { get; set; }


    public Event<ICreateCargo> CreateCargoEvent { get; private set; }
    public Event<ISendSelfie> SendSelfieEvent { get; private set; }
    public Event<ICargoApproved> CargoApprovedEvent { get; private set; }
    public Event<ICargoRejected> CargoRejectedEvent { get; private set; }

    public Event<IStartRoute> StartRouteEvent { get; private set; }
    public Event<IRouteConfirmed> RouteConfirmedEvent { get; private set; }
    public Event<IAutoRoute> AutoRouteEvent { get; private set; }
    public Event<IManuelRoute> ManuelRouteEvent { get; private set; }

    public Event<IStartDelivery> StartDeliveryEvent { get; private set; }    
    public Event<ICreateDelivery> CreateDeliveryEvent { get; private set; }
    public Event<ICreateDelivery> CardPaymentEvent { get; private set; }
    public Event<ICreateDelivery> FreeDeliveryEvent { get; private set; }
    public Event<ICreateDelivery> PayAtDoorEvent { get; private set; }

    public Event<INotDelivered> NotDeliveredEvent { get; private set; }
    public Event<ICreateRefund> CreateRefundEvent { get; private set; }
    public Event<IDeliveryCompleted> DeliveryCompletedEvent { get; private set; }


    [Obsolete]
    public CargoStateMachine()
    {
        QueueConfigurationExtensions.AddQueueConfiguration(null, out IQueueConfiguration queueConfiguration);
        InstanceState(instance => instance.CurrentState);

        Event(() => CreateCargoEvent, instance => instance.CorrelateBy<Guid>(state => state.CargoId, context => context.Message.CargoId).SelectId(s => Guid.NewGuid()));
        Event(() => SendSelfieEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CargoApprovedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CargoRejectedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
       
        Event(() => StartRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => RouteConfirmedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => AutoRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => ManuelRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        
        Event(() => StartDeliveryEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CardPaymentEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => FreeDeliveryEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => PayAtDoorEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CreateDeliveryEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => NotDeliveredEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CreateRefundEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => DeliveryCompletedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        Initially(
            When(CreateCargoEvent)
                .Then(context =>
                {
                    context.Instance.UserId = context.Data.UserId;
                    context.Instance.CargoId = context.Data.CargoId;
                    context.Instance.CreatedOn = DateTime.Now;
                })
                .TransitionTo(CreateCargo)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateCargo]}"), context => new CreateCargoCommand(context.Instance.CorrelationId)
                {
                    CargoId = context.Data.CargoId,
                    UserId = context.Data.UserId,
                }));

        #region Cargo

        During(CreateCargo,
             When(SendSelfieEvent)
                 .TransitionTo(SendSelfie)
                 .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.SendSelfie]}"), context => new SendSelfieCommand(context.Data.CorrelationId)
                 {
                     CargoId = context.Instance.CargoId,
                     CorrelationId = context.Instance.CorrelationId
                 }));

        During(SendSelfie,
           When(CargoApprovedEvent)
               .TransitionTo(CargoApproved)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoApproved]}"), context => new CargoApprovedCommand(context.Data.CorrelationId)
               {
                   CargoId = context.Instance.CargoId,
                   CorrelationId = context.Instance.CorrelationId
               }));

        During(CargoApproved,
            When(StartRouteEvent)
                .TransitionTo(StartRoute)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartRoute]}"), context => new StartRouteCommand(context.Data.CorrelationId)
                {
                    CargoId = context.Instance.CargoId,
                    CorrelationId = context.Instance.CorrelationId
                }),
            When(CargoRejectedEvent)
                .TransitionTo(CargoRejected)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoRejected]}"), context => new CargoRejectedCommand(context.Data.CorrelationId)
                {
                    CargoId = context.Instance.CargoId,
                    CorrelationId = context.Instance.CorrelationId
                }));

        #endregion

        #region Start Route

        During(StartRoute,
            When(RouteConfirmedEvent)
                .TransitionTo(RouteConfirmed)
                 .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.RouteConfirmed]}"), context => new RouteConfirmedCommand(context.Data.CorrelationId)
                 {
                     CargoId = context.Instance.CargoId,
                     CorrelationId = context.Instance.CorrelationId
                 })
            );

        During(RouteConfirmed,
            When(AutoRouteEvent)
                .TransitionTo(AutoRoute)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.AutoRoute]}"), context => new AutoRouteCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId
                }),
            When(ManuelRouteEvent)
                .TransitionTo(ManuelRoute)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.ManuelRoute]}"), context => new ManuelRouteCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId
                })
            );

        During(AutoRoute,
         When(StartDeliveryEvent)
             .TransitionTo(StartDelivery)
             .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartDelivery]}"), context => new StartDeliveryCommand(context.Data.CorrelationId)
             {
                 CorrelationId = context.Instance.CorrelationId
             }));

        During(ManuelRoute,
         When(StartDeliveryEvent)
             .TransitionTo(StartDelivery)
             .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartDelivery]}"), context => new StartDeliveryCommand(context.Data.CorrelationId)
             {
                 CorrelationId = context.Instance.CorrelationId
             }));

        #endregion

        #region Delivery

        #region Start Delivery

        During(StartDelivery,
           When(CreateDeliveryEvent)
               .TransitionTo(CreateDelivery)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateDelivery]}"), context => new CreateDeliveryCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CargoId = context.Instance.CargoId
               }),
           When(NotDeliveredEvent)
               .TransitionTo(NotDelivered)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.NotDelivered]}"), context => new NotDeliveredCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CargoId = context.Instance.CargoId
               }),
           When(CreateRefundEvent)
               .TransitionTo(CreateRefund)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateRefund]}"), context => new CreateRefundCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CargoId = context.Instance.CargoId
               })
           );

        #endregion

        #region CreateDelivery

        During(CreateDelivery,
          When(CardPaymentEvent)
              .TransitionTo(CardPayment)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CardPayment]}"), context => new CardPaymentCommand(context.Data.CorrelationId)
               {
                   CargoId = context.Instance.CargoId,
                   CorrelationId = context.Instance.CorrelationId
               }),
           When(FreeDeliveryEvent)
              .TransitionTo(FreeDelivery)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.FreeDelivery]}"), context => new FreeDeliveryCommand(context.Data.CorrelationId)
               {
                   CargoId = context.Instance.CargoId,
                   CorrelationId = context.Instance.CorrelationId
               }),
            When(PayAtDoorEvent)
              .TransitionTo(PayAtDoor)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.PayAtDoor]}"), context => new PayAtDoorCommand(context.Data.CorrelationId)
               {
                   CargoId = context.Instance.CargoId,
                   CorrelationId = context.Instance.CorrelationId
               })
          );

        During(CardPayment,
         When(DeliveryCompletedEvent)
             .TransitionTo(DeliveryCompleted)
              .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
              {
                  CargoId = context.Instance.CargoId,
                  CorrelationId = context.Instance.CorrelationId
              })
         );

        During(FreeDelivery,
         When(DeliveryCompletedEvent)
             .TransitionTo(DeliveryCompleted)
              .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
              {
                  CargoId = context.Instance.CargoId,
                  CorrelationId = context.Instance.CorrelationId
              })
         );

        During(PayAtDoor,
         When(DeliveryCompletedEvent)
             .TransitionTo(DeliveryCompleted)
              .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
              {
                  CargoId = context.Instance.CargoId,
                  CorrelationId = context.Instance.CorrelationId
              })
         );
        #endregion

        #region NotDelivered

        During(NotDelivered,
          When(DeliveryCompletedEvent)
              .TransitionTo(DeliveryCompleted)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
               {
                   CargoId = context.Instance.CargoId,
                   CorrelationId = context.Instance.CorrelationId
               })
          );

        #endregion

        #region CreateRefund

        During(CreateRefund,
           When(DeliveryCompletedEvent)
               .TransitionTo(DeliveryCompleted)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
                {
                    CargoId = context.Instance.CargoId,
                    CorrelationId = context.Instance.CorrelationId
                })
           );

        #endregion

        During(DeliveryCompleted,
           When(StartDeliveryEvent)
               .TransitionTo(StartDelivery)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartDelivery]}"), context => new StartDeliveryCommand(context.Data.CorrelationId)
                {
                    CargoId = context.Instance.CargoId,
                    CorrelationId = context.Instance.CorrelationId
                })
           );

        #endregion

        SetCompletedWhenFinalized();
    }
}
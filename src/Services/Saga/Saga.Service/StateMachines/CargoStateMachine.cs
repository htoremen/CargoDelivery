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

    public State CreateDebit { get; set; }
    public State CreateDebitFault { get; set; }
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

    public Event<ICreateDebit> CreateDebitEvent { get; private set; }
    public Event<Fault<ICreateDebit>> CreateDebitFaultEvent { get; private set; }
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

        SetCorrelationId();

        Initially(ProcessApplication(queueConfiguration));

        During(CreateDebit, SendSelfieActivity(queueConfiguration));

        During(SendSelfie, SendSelfieActivity(queueConfiguration), CargoApprovalActivity(queueConfiguration));
        During(CargoApproval, StartRouteActivity(queueConfiguration), CargoRejectedActivity(queueConfiguration));

        During(StartRoute, AutoRouteActivity(queueConfiguration), ManuelRouteActivity(queueConfiguration));
        During(AutoRoute, StartDeliveryActivity(queueConfiguration));
        During(ManuelRoute, StartDeliveryActivity(queueConfiguration));

        During(StartDelivery, NewDeliveryActivity(queueConfiguration));
        During(NewDelivery, CreateDeliveryActivity(queueConfiguration), NotDeliveredActivity(queueConfiguration), CreateRefundActivity(queueConfiguration));
        During(CreateDelivery, CardPaymentActivity(queueConfiguration), FreeDeliveryActivity(queueConfiguration), PayAtDoorActivity(queueConfiguration));  
        
        During(CardPayment, DeliveryCompletedActivity(queueConfiguration));
        During(FreeDelivery, DeliveryCompletedActivity(queueConfiguration));
        During(PayAtDoor, DeliveryCompletedActivity(queueConfiguration));

        During(NotDelivered, DeliveryCompletedActivity(queueConfiguration));
        During(CreateRefund, DeliveryCompletedActivity(queueConfiguration));

        During(DeliveryCompleted, NewDeliveryActivity(queueConfiguration));
        During(DeliveryCompleted, ShiftCompletionActivity(queueConfiguration));

        SetCompletedWhenFinalized();
    }

    private EventActivities<CargoStateInstance> CreateDebitFaultActivity(IQueueConfiguration queueConfiguration)
    {
        return When(CreateDebitFaultEvent)
                 .TransitionTo(CreateDebitFault)
                 .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateDebitFault]}"), context => new CreateDebitFaultCommand(context.Instance.CorrelationId)
                 {
                     DebitId = context.Data.Message.DebitId,
                     CourierId = context.Data.Message.CourierId,
                     Cargos = context.Data.Message.Cargos,
                     CurrentState = context.Instance.CurrentState,
                     Exceptions = context.Data.Exceptions
                 });
    }

    private EventActivities<CargoStateInstance> ShiftCompletionActivity(IQueueConfiguration queueConfiguration)
    {
        return When(ShiftCompletionEvent)
              .TransitionTo(ShiftCompletion)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.ShiftCompletion]}"), context => new ShiftCompletionCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
               }).Finalize();
    }

    private EventActivities<CargoStateInstance> DeliveryCompletedActivity(IQueueConfiguration queueConfiguration)
    {
        return When(DeliveryCompletedEvent)
             .TransitionTo(DeliveryCompleted)
              .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.DeliveryCompleted]}"), context => new DeliveryCompletedCommand(context.Data.CorrelationId)
              {
                  CorrelationId = context.Instance.CorrelationId,
                  CurrentState = context.Instance.CurrentState,
              });
    }



    #region CreateDelivery During

    [Obsolete]
    private EventActivities<CargoStateInstance> PayAtDoorActivity(IQueueConfiguration queueConfiguration)
    {
        return When(PayAtDoorEvent)
              .TransitionTo(PayAtDoor)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.PayAtDoor]}"), context => new PayAtDoorCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CargoId = context.Data.CargoId,
                   CurrentState = context.Instance.CurrentState,
                   PaymentType = context.Data.PaymentType
               });
    }

    [Obsolete]
    private EventActivities<CargoStateInstance> FreeDeliveryActivity(IQueueConfiguration queueConfiguration)
    {
        return When(FreeDeliveryEvent)
              .TransitionTo(FreeDelivery)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.FreeDelivery]}"), context => new FreeDeliveryCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CargoId = context.Data.CargoId,
                   CurrentState = context.Instance.CurrentState,
                   PaymentType = context.Data.PaymentType
               });
    }

    private EventActivities<CargoStateInstance> CardPaymentActivity(IQueueConfiguration queueConfiguration)
    {
        return When(CardPaymentEvent)
              .TransitionTo(CardPayment)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CardPayment]}"), context => new CardPaymentCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CargoId = context.Data.CargoId,
                   CurrentState = context.Instance.CurrentState,
                   PaymentType = context.Data.PaymentType
               });
    }

    #endregion

    #region Start Delivery During

    [Obsolete]
    private EventActivities<CargoStateInstance> CreateRefundActivity(IQueueConfiguration queueConfiguration)
    {
        return When(CreateRefundEvent)
               .TransitionTo(CreateRefund)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateRefund]}"), context => new CreateRefundCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
                   CargoId = context.Data.CargoId
               });
    }

    [Obsolete]
    private EventActivities<CargoStateInstance> NotDeliveredActivity(IQueueConfiguration queueConfiguration)
    {
        return When(NotDeliveredEvent)
               .TransitionTo(NotDelivered)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.NotDelivered]}"), context => new NotDeliveredCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
                   CargoId = context.Data.CargoId
               });
    }

    [Obsolete]
    private EventActivities<CargoStateInstance> CreateDeliveryActivity(IQueueConfiguration queueConfiguration)
    {
        return When(CreateDeliveryEvent)
               .TransitionTo(CreateDelivery)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateDelivery]}"), context => new CreateDeliveryCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState,
                   CargoId = context.Data.CargoId,
                   PaymentType = context.Data.PaymentType
               });
    }

    [Obsolete]
    private EventActivities<CargoStateInstance> NewDeliveryActivity(IQueueConfiguration queueConfiguration)
    {
        return When(NewDeliveryEvent)
                 .TransitionTo(NewDelivery)
                 .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.NewDelivery]}"), context => new NewDeliveryCommand(context.Data.CorrelationId)
                 {
                     CorrelationId = context.Instance.CorrelationId,
                     CurrentState = context.Instance.CurrentState
                 });
    }

    #endregion

    #region Start Route During

    private EventActivities<CargoStateInstance> StartDeliveryActivity(IQueueConfiguration queueConfiguration)
    {
        return When(StartDeliveryEvent)
             .TransitionTo(StartDelivery)
             .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartDelivery]}"), context => new StartDeliveryCommand(context.Data.CorrelationId)
             {
                 CorrelationId = context.Instance.CorrelationId,
                 CurrentState = context.Instance.CurrentState,
             });
    }

    private EventActivities<CargoStateInstance> ManuelRouteActivity(IQueueConfiguration queueConfiguration)
    {
        return When(ManuelRouteEvent)
                .TransitionTo(ManuelRoute)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.ManuelRoute]}"), context => new ManuelRouteCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId,
                    CurrentState = context.Instance.CurrentState
                });
    }

    private EventActivities<CargoStateInstance> AutoRouteActivity(IQueueConfiguration queueConfiguration)
    {
        return When(AutoRouteEvent)
                .TransitionTo(AutoRoute)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.AutoRoute]}"), context => new AutoRouteCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId,
                    CurrentState = context.Instance.CurrentState

                });
    }

    #endregion

    #region Cargo During

    private EventActivities<CargoStateInstance> CargoRejectedActivity(IQueueConfiguration queueConfiguration)
    {
        return When(CargoRejectedEvent)
                .TransitionTo(CargoRejected)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoRejected]}"), context => new CargoRejectedCommand(context.Data.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId,
                    CurrentState = context.Instance.CurrentState
                });
    }

    private EventActivities<CargoStateInstance> StartRouteActivity(IQueueConfiguration queueConfiguration)
    {
        return When(StartRouteEvent)
               .TransitionTo(StartRoute)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.StartRoute]}"), context => new StartRouteCommand(context.Data.CorrelationId)
               {
                   CurrentState = context.Instance.CurrentState,
                   CorrelationId = context.Instance.CorrelationId
               });
    }

    [Obsolete]
    private EventActivities<CargoStateInstance> CargoApprovalActivity(IQueueConfiguration queueConfiguration)
    {
        return When(CargoApprovalEvent)
               .TransitionTo(CargoApproval)
               .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoApproval]}"), context => new CargoApprovalCommand(context.Data.CorrelationId)
               {
                   CorrelationId = context.Instance.CorrelationId,
                   CurrentState = context.Instance.CurrentState
               });
    }

    [Obsolete]
    private EventActivities<CargoStateInstance> SendSelfieActivity(IQueueConfiguration queueConfiguration)
    {
        return When(SendSelfieEvent)
                 .TransitionTo(SendSelfie)
                 .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.SendSelfie]}"), context => new SendSelfieCommand(context.Data.CorrelationId)
                 {
                     CorrelationId = context.Instance.CorrelationId,
                     CurrentState = context.Instance.CurrentState
                 });
    }

  
    #endregion



    private void SetCorrelationId()
    {
        #region Event

        Event(() => CreateDebitEvent, instance => instance.CorrelateBy<Guid>(state => state.CourierId, context => context.Message.DebitId).SelectId(s => Guid.NewGuid()));
        Event(() => CreateDebitFaultEvent, instance => instance.CorrelateById(selector => selector.Message.Message.CorrelationId)
                                                               .SelectId(selector => selector.Message.Message.DebitId));

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
    }

    /// <summary>
    /// https://github.com/i-akash/distributed-transaction-patterns/blob/main/SyncSaga/StateMachine/LoanStateManchine.cs
    /// </summary>
    /// <param name="queueConfiguration"></param>
    /// <returns></returns>
    [Obsolete]
    public EventActivityBinder<CargoStateInstance, ICreateDebit> ProcessApplication(IQueueConfiguration queueConfiguration)
    {
        return When(CreateDebitEvent)
                .Then(context =>
                {
                    context.Instance.CourierId = context.Data.CourierId;
                    context.Instance.CreatedOn = DateTime.Now;
                })
                .TransitionTo(CreateDebit)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateDebit]}"), context => new CreateDebitCommand(context.Instance.CorrelationId)
                {
                    DebitId = context.Data.DebitId,
                    CourierId = context.Data.CourierId,
                    Cargos = context.Data.Cargos,
                    CurrentState = context.Instance.CurrentState                    
                });
    }
}
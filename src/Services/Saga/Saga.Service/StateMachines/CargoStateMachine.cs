using Core.Domain;
using Core.Domain.Enums;
using Cargos;
using MassTransit;
using Saga.Application.Cargos;
using Saga.Domain.Instances;
using Routes;
using Saga.Application.Routes;

namespace Saga.Service.StateMachines;

public class CargoStateMachine : MassTransitStateMachine<CargoStateInstance>
{
    public State CreateCargo { get; set; }
    public State CreateSelfie { get; set; }
    public State CargoSendApproved { get; set; }
    public State CargoApproved { get; set; }
    public State CargoRejected { get; set; }

    public State RouteConfirmed { get; set; }
    public State AutoRoute { get; set; }
    public State ManuelRoute { get; set; }



    public Event<ICreateCargo> CreateCargoEvent { get; private set; }
    public Event<ICreateSelfie> CreateSelfieEvent { get; private set; }
    public Event<ICargoSendApproved> CargoSendApprovedEvent { get; private set; }
    public Event<ICargoApproved> CargoApprovedEvent { get; private set; }
    public Event<ICargoRejected> CargoRejectedEvent { get; private set; }

    public Event<IRouteConfirmed> RouteConfirmedEvent { get; private set; }
    public Event<IAutoRoute> AutoRouteEvent { get; private set; }
    public Event<IManuelRoute> ManuelRouteEvent { get; private set; }



    [Obsolete]
    public CargoStateMachine()
    {
        QueueConfigurationExtensions.AddQueueConfiguration(null, out IQueueConfiguration queueConfiguration);
        InstanceState(instance => instance.CurrentState);

        Event(() => CreateCargoEvent, instance => instance.CorrelateBy<Guid>(state => state.CargoId, context => context.Message.CargoId).SelectId(s => Guid.NewGuid()));
        Event(() => CreateSelfieEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CargoSendApprovedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CargoApprovedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => CargoRejectedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => RouteConfirmedEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => AutoRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => ManuelRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));


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
             When(CreateSelfieEvent)
                 .TransitionTo(CreateSelfie)
                 .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateSelfie]}"), context => new CreateSelfieCommand(context.Data.CorrelationId)
                 {
                     CargoId = context.Instance.CargoId,
                     CorrelationId = context.Instance.CorrelationId
                 }));

        During(CreateSelfie,
         When(CargoSendApprovedEvent)
             .TransitionTo(CargoSendApproved)
             .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoSendApproved]}"), context => new CargoSendApprovedCommand(context.Data.CorrelationId)
             {
                 CargoId = context.Instance.CargoId,
                 CorrelationId = context.Instance.CorrelationId
             }));

        During(CargoSendApproved,
            When(CargoApprovedEvent)
                .TransitionTo(CargoApproved)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoApproved]}"), context => new CargoApprovedCommand(context.Data.CorrelationId)
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

        #region Route

        During(CargoApproved,
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

        #endregion

        SetCompletedWhenFinalized();
    }
}
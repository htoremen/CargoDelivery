using Cargos;
using Core.Domain;
using Core.Domain.Enums;
using MassTransit;
using Routes;
using Saga.Application.Routes;
using Saga.Domain.Instances;

namespace Saga.Service.StateMachines;

public class RouteStateMachine : MassTransitStateMachine<RouteStateInstance>
{
    public State RouteConfirmed { get; set; }
    public State AutoRoute { get; set; }
    public State ManuelRoute { get; set; }

    public Event<IRouteConfirmed> RouteConfirmedEvent { get; private set; }
    public Event<IAutoRoute> AutoRouteEvent { get; private set; }
    public Event<IManuelRoute> ManuelRouteEvent { get; private set; }

    public RouteStateMachine()
    {
        QueueConfigurationExtensions.AddQueueConfiguration(null, out IQueueConfiguration queueConfiguration);
        InstanceState(instance => instance.CurrentState);

        Event(() => RouteConfirmedEvent, instance => instance.CorrelateBy<Guid>(state => state.CargoId, context => context.Message.CargoId).SelectId(s => Guid.NewGuid()));
        Event(() => AutoRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
        Event(() => ManuelRouteEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));

        Initially(
            When(RouteConfirmedEvent)
            .Then(context =>
            {
                context.Instance.UserId = context.Data.UserId;
                context.Instance.CargoId = context.Data.CargoId;
                context.Instance.CreatedOn = DateTime.Now;
            })
                .TransitionTo(RouteConfirmed)
                .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateCargo]}"), context => new RouteConfirmedCommand(context.Instance.CorrelationId)
                {
                    CargoId = context.Data.CargoId,
                    UserId = context.Data.UserId,
                }));

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


        SetCompletedWhenFinalized();
    }
}
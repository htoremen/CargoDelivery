using Core.Domain.Events.Cargos;
using MassTransit;
using Saga.Domain.Instances;

namespace Saga.Service.StateMachines;

public class CargoStateMachine : MassTransitStateMachine<CargoStateInstance>
{
    public CargoStateMachine()
    {
        InstanceState(instance => instance.CurrentState);

        Event(() => CreateCargo, instance => instance
                .CorrelateBy<Guid>(state => state.CargoId, context => context.Message.CargoId)
                .SelectId(s => Guid.NewGuid()));

        Event(() => CreateSelfie, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CargoSendApproved, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CargoApproved, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CargoRejected, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));


    }

    public State CreateCargoState { get; set; }
    public State CreateSelfieState { get; set; }
    public State CargoSendApprovedState { get; set; }
    public State CargoApprovedState { get; set; }
    public State CargoRejectedState { get; set; }


    public Event<ICreateCargo> CreateCargo { get; set; }
    public Event<ICreateSelfie> CreateSelfie { get; set; }
    public Event<ICargoSendApproved> CargoSendApproved { get; set; }
    public Event<ICargoApproved> CargoApproved { get; set; }
    public Event<ICargoRejected> CargoRejected { get; set; }
}


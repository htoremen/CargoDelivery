using Core.Domain.Events.Cargos;
using MassTransit;
using Saga.Domain.Instances;

namespace Saga.Service.StateMachines;

public class CargoStateMachine : MassTransitStateMachine<CargoStateInstance>
{
    public CargoStateMachine()
    {
        InstanceState(instance => instance.CurrentState);

        Event(() => CreateCargoEvent, instance => instance
                .CorrelateBy<Guid>(state => state.CargoId, context => context.Message.CargoId)
                .SelectId(s => Guid.NewGuid()));
    }

    public State CreateCargo { get; set; }
    public State CreateSelfie { get; set; }
    public State CargoSendApproved { get; set; }
    public State CargoApproved { get; set; }
    public State CargoRejected { get; set; }


    public Event<ICreateCargo> CreateCargoEvent { get; set; }
    public Event<ICreateSelfie> CreateSelfieEvent { get; set; }
    public Event<ICargoSendApproved> CargoSendApprovedEvent { get; set; }
    public Event<ICargoApproved> CargoApprovedEvent { get; set; }
    public Event<ICargoRejected> CargoRejectedEvent { get; set; }
}


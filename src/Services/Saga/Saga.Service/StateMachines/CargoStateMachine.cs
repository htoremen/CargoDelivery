using Core.Domain.Enums;
using Core.Domain.Events.Cargos;
using MassTransit;
using Saga.Application.Cargos;
using Saga.Domain.Instances;

namespace Saga.Service.StateMachines;

public class CargoStateMachine : MassTransitStateMachine<CargoStateInstance>
{
    public State CreateCargo { get; set; }
    public State CreateSelfie { get; set; }
    public State CargoSendApproved { get; set; }
    public State CargoApproved { get; set; }
    public State CargoRejected { get; set; }

    public Event<ICreateCargo> CreateCargoCommand { get; set; }
    public Event<ICreateSelfie> CreateSelfieCommand { get; set; }
    public Event<ICargoSendApproved> CargoSendApprovedCommand { get; set; }
    public Event<ICargoApproved> CargoApprovedCommand { get; set; }
    public Event<ICargoRejected> CargoRejectedCommand { get; set; }

    public CargoStateMachine()
    {
        InstanceState(instance => instance.CurrentState);

        Event(() => CreateCargoCommand, instance => instance
                .CorrelateBy<Guid>(state => state.CargoId, context => context.Message.CargoId)
                .SelectId(s => Guid.NewGuid()));

        Event(() => CreateSelfieCommand, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CargoSendApprovedCommand, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CargoApprovedCommand, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CargoRejectedCommand, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));


        Initially(When(CreateCargoCommand)
        .Then(context =>
        {
            context.Instance.UserId = context.Data.UserId;
            context.Instance.CargoId = context.Data.CargoId;
            context.Instance.CreatedOn = DateTime.Now;
        })
        .TransitionTo(CreateCargo)
        .Send(new Uri($"queue:{QueueName.CreateCargo.ToString()}"), context => new CreateCargoCommand(context.Instance.CorrelationId)
        {
            CargoId = context.Data.CargoId,
            UserId = context.Data.UserId,
        }));

        During(CreateCargo,
         When(CreateSelfieCommand)
         .TransitionTo(CreateSelfie)
         .Send(new Uri($"queue:{QueueName.CreateSelfie}"), context => new CreateSelfieCommand(context.Instance.CorrelationId)
         {
             CargoId = context.Instance.CargoId,
         }));


        During(CargoSendApproved,
            When(CargoApprovedCommand)
            .TransitionTo(CargoApproved)
            .Send(new Uri($"queue:{QueueName.CargoApproved}"), context => new CargoApprovedCommand(context.Instance.CorrelationId)
            {
                CargoId = context.Instance.CargoId,
            })
            .Finalize(),
            When(CargoRejectedCommand)
            .TransitionTo(CargoRejected)
            .Send(new Uri($"queue:{QueueName.CargoRejected}"), context => new CargoRejectedCommand
            {
                CargoId = context.Instance.CargoId,
            }));

        SetCompletedWhenFinalized();
    }


}


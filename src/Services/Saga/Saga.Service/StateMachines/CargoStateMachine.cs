using Core.Domain;
using Core.Domain.Enums;
using Cargos;
using MassTransit;
using Saga.Application.Cargos;
using Saga.Domain.Instances;

namespace Saga.Service.StateMachines;

public class CargoStateMachine : MassTransitStateMachine<CargoStateInstance>
{
    public State CreateCargo { get; set; }

    public State CreateSelfie { get; set; }
   // public State CreateSelfieFault { get; set; }

    public State CargoSendApproved { get; set; }
    public State CargoApproved { get; set; }
    public State CargoRejected { get; set; }

    public Event<ICreateCargo> CreateCargoEvent { get; private set; }
    public Event<ICreateSelfie> CreateSelfieEvent { get; private set; }
    //public Event<Fault<ICreateSelfie>> CreateSelfieFaultEvent { get; private set; }
    public Event<ICargoSendApproved> CargoSendApprovedEvent { get; private set; }
    public Event<ICargoApproved> CargoApprovedEvent { get; private set; }
    public Event<ICargoRejected> CargoRejectedEvent { get; private set; }

    public CargoStateMachine()
    {
        QueueConfigurationExtensions.AddQueueConfiguration(null, out IQueueConfiguration queueConfiguration);
        InstanceState(instance => instance.CurrentState);

        Event(() => CreateCargoEvent, instance => instance.CorrelateBy<Guid>(state => state.CargoId, context => context.Message.CargoId).SelectId(s => Guid.NewGuid()));

        Event(() => CreateSelfieEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));
       // Event(() => CreateSelfieFaultEvent, instance => instance.CorrelateById(m => m.Message.Message.CorrelationId).SelectId(m => m.Message.Message.CorrelationId));

        Event(() => CargoSendApprovedEvent, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CargoApprovedEvent, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));

        Event(() => CargoRejectedEvent, instance => instance
              .CorrelateById(selector => selector.Message.CorrelationId));


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

        During(CreateCargo,
         When(CreateSelfieEvent)
         .TransitionTo(CreateSelfie)
         .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateSelfie]}"), context => new CreateSelfieCommand(context.Data.CorrelationId)
         {
             CargoId = context.Instance.CargoId,
             CorrelationId = context.Instance.CorrelationId
         })
         );

        During(CreateSelfie,
         When(CargoSendApprovedEvent)
         .TransitionTo(CargoSendApproved)
         .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoSendApproved]}"), context => new CargoSendApprovedCommand(context.Data.CorrelationId)
         {
             CargoId = context.Instance.CargoId,
             CorrelationId = context.Instance.CorrelationId
         })
        // ,
         //When(CreateSelfieFaultEvent)
         //   .TransitionTo(CreateSelfieFault)
         //    .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CreateSelfieFault]}"), context => new CreateSelfieFaultCommand(context.Message.Message.CorrelationId)
         //    {
         //        CargoId = context.Instance.CargoId,
         //        CorrelationId = context.Instance.CorrelationId
         //    })
         );

        During(CargoSendApproved,
            When(CargoApprovedEvent)
            .TransitionTo(CargoApproved)
            .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoApproved]}"), context => new CargoApprovedCommand(context.Data.CorrelationId)
            {
                CargoId = context.Instance.CargoId,
                CorrelationId = context.Instance.CorrelationId
            }),
           // .Finalize(),
            When(CargoRejectedEvent)
            .TransitionTo(CargoRejected)
            .Send(new Uri($"queue:{queueConfiguration.Names[QueueName.CargoRejected]}"), context => new CargoRejectedCommand(context.Data.CorrelationId)
            {
                CargoId = context.Instance.CargoId,
                CorrelationId = context.Instance.CorrelationId
            }));

        SetCompletedWhenFinalized();
    }


}
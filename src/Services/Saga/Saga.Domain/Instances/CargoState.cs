using MassTransit;

namespace Saga.Domain.Instances;

public class CargoState : 
    SagaStateMachineInstance,
    ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedOn { get; set; }

    public int Version { get; set; }
}
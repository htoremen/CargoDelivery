using MassTransit;

namespace Saga.Domain.Instances;

public class CargoStateInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedOn { get; set; }
}

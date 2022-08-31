using MassTransit;

namespace Saga.Domain.Instances;

public class RouteStateInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedOn { get; set; }
}

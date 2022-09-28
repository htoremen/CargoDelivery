using Enums;
using MassTransit;

namespace Saga.Domain.Instances;

public class CargoStateInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid CourierId { get; set; }
    public DateTime CreatedOn { get; set; }
}
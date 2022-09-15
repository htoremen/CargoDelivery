using Core.Domain.Instances;
using Enums;
using MassTransit;

namespace Saga.Domain.Instances;

public class CargoStateInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedOn { get; set; }
    public PaymentType PaymentType { get; set; }
    public List<CargoRouteInstance> CargoRoutes { get; set; }
}
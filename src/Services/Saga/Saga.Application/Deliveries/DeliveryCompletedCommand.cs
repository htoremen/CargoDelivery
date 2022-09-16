namespace Saga.Application.Deliveries;

public class DeliveryCompletedCommand : IDeliveryCompleted
{
    public DeliveryCompletedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public string CurrentState { get; set; }

    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

namespace Saga.Application.Deliveries;

public class DeliveryCompletedCommand : IDeliveryCompleted
{
    public DeliveryCompletedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}

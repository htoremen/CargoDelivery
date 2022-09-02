namespace Delivery.Application.Deliveries.DeliveryCompleteds;

public class DeliveryCompletedResponse
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

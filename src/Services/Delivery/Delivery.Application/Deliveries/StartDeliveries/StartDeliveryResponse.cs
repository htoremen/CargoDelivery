namespace Delivery.Application.Deliveries.StartDeliveries;

public class StartDeliveryResponse
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

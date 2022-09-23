namespace Delivery.Application.Deliveries.Commands.InsertDeliveries;

public class InsertDeliveryResponse
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

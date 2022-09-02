namespace Order.Application.Deliveries.NotDeliveries;

public class NotDeliveryResponse
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}


namespace Delivery.Application.Deliveries.NotDelivereds;

public class NotDeliveredResponse
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}


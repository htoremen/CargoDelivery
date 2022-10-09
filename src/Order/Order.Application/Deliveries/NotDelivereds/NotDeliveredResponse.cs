namespace Order.Application.Deliveries.NotDelivereds;

public class NotDeliveredResponse
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}


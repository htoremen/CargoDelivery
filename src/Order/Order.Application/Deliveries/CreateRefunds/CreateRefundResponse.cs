namespace Order.Application.Deliveries.CreateRefunds;

public class CreateRefundResponse
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

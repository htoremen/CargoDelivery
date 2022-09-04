namespace Order.Application.Payments.FreeDeliveries;

public class FreeDeliveryResponse
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

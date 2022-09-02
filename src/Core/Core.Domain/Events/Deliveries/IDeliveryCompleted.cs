namespace Deliveries;

public interface IDeliveryCompleted
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
public class DeliveryCompleted : IDeliveryCompleted
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
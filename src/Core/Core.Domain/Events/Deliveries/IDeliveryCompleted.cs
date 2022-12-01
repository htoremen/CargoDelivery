namespace Events;

public interface IDeliveryCompleted
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
public class DeliveryCompleted : IDeliveryCompleted
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid CargoId { get; set; }
}
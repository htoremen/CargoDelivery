namespace Events;

public interface INotDelivered
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class NotDelivered : INotDelivered
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
    public string CurrentState { get; set; }
}
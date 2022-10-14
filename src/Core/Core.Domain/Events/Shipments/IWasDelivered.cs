namespace Shipments;

public interface IWasDelivered
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class WasDelivered : IWasDelivered
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
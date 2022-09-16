namespace Deliveries;

public interface IStartDelivery
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class StartDelivery : IStartDelivery
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
namespace Events;

public interface INewDelivery
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class NewDelivery : INewDelivery
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
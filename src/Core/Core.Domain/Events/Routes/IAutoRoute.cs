namespace Events;

public interface IAutoRoute //: IEvent
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
}

public class AutoRoute : IAutoRoute
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
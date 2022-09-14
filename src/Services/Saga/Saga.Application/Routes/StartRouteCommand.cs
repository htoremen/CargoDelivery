namespace Saga.Application.Routes;

public class StartRouteCommand : IStartRoute
{
    public StartRouteCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

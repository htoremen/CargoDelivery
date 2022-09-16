namespace Saga.Application.Routes;
public class AutoRouteCommand : IAutoRoute
{
    public AutoRouteCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public string CurrentState { get; set; }

    public Guid CorrelationId { get; set; }
}


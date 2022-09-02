namespace Saga.Application.Routes;

public class RouteCompletedCommand : IRouteCompleted
{
    public RouteCompletedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}

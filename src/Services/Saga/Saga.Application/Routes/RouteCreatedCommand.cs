namespace Saga.Application.Routes;

public class RouteCreatedCommand : IRouteCreated
{
    public RouteCreatedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }
    public Guid CargoId { get; set; }

    public Guid CorrelationId { get; set; }
}

namespace Saga.Application.Routes;


public class RouteConfirmedCommand : IRouteConfirmed
{
    public RouteConfirmedCommand(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }

    public Guid CorrelationId { get; set; }
}

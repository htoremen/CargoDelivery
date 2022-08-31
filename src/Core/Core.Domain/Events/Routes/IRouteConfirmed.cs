namespace Routes;
public interface IRouteConfirmed : IEvent
{
    public Guid CorrelationId { get; }
    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
}


public class RouteConfirmed : IRouteConfirmed
{
    public Guid CorrelationId { get; }
    public Guid CargoId { get; set; }
    public Guid UserId { get; set; }
}
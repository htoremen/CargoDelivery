namespace Core.Domain.Events.Routes;
public interface IRouteConfirmed : IEvent
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}


public class RouteConfirmed : IRouteConfirmed
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
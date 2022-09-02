namespace Routes;

public interface IRouteCompleted
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class RouteCompleted : IRouteCompleted
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}
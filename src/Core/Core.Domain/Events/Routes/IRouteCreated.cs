namespace Routes;

public interface IRouteCreated
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class RouteCreated : IRouteCreated
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

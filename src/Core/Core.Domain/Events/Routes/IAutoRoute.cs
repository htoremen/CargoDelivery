namespace Core.Domain.Events.Routes;

public interface IAutoRoute : IEvent
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class AutoRoute : IAutoRoute
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
namespace Routes;

public interface IManuelRoute //: IEvent
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class ManuelRoute : IManuelRoute
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
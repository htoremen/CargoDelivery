namespace Cargos;

public interface IStartRoute //: IEvent
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class StartRoute : IStartRoute
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
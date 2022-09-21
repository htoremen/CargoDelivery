using Core.Domain.Instances;

namespace Cargos;

public interface IStartRoute //: IEvent
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}

public class StartRoute : IStartRoute
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
}
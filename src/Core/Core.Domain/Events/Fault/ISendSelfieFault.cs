namespace Core.Domain.Events.Fault;

public interface ISendSelfieFault //: IEvent
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class SendSelfieFault : ISendSelfieFault
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
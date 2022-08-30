namespace Core.Domain.Events.Fault;

public interface ICreateSelfieFault : IEvent
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class CreateSelfieFault : ICreateSelfieFault
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
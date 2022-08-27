namespace Core.Domain.Events.Cargos;

public interface ICreateSelfie : IEvent
{
    public Guid OrderId { get; set; }
}

public class CreateSelfie : ICreateSelfie
{
    public Guid CorrelationId { get; set; }
    public Guid OrderId { get; set; }
}
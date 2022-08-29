namespace Cargos;
public interface ICreateSelfie : IEvent
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class CreateSelfie : ICreateSelfie
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
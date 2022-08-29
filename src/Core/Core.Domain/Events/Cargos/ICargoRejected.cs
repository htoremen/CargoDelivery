namespace Cargos;
public interface ICargoRejected : IEvent
{
    public Guid CorrelationId { get; }
    public Guid CargoId { get; set; }
}

public class CargoRejected : ICargoRejected
{
    public Guid CorrelationId { get; }
    public Guid CargoId { get; set; }
}
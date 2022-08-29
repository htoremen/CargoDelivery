namespace Cargos;
public interface ICargoApproved : IEvent
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; }
}

public class CargoApproved : ICargoApproved
{
    public Guid CorrelationId { get; }
    public Guid CargoId { get; set; }
}
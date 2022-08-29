namespace Core.Domain.Events.Cargos;
public interface ICargoApproved : IEvent
{
    public Guid CargoId { get; set; }
}

public class CargoApproved : ICargoApproved
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
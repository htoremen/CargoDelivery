namespace Core.Domain.Events.Cargos;

public interface ICargoRejected : IEvent
{
    public Guid CargoId { get; set; }
}

public class CargoRejected : ICargoRejected
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
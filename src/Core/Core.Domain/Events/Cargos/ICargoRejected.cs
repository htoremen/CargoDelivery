namespace Core.Domain.Events.Cargos;

public interface ICargoRejected : IEvent
{
    public Guid OrderId { get; set; }
}

public class CargoRejected : ICargoRejected
{
    public Guid CorrelationId { get; set; }
    public Guid OrderId { get; set; }
}
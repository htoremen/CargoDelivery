namespace Core.Domain.Events.Sagas;

public interface ICargoSagaError : IEvent
{
    public Guid CargoId { get; set; }
    public Guid CorrelationId { get; set; }
}

public class CargoSagaError : ICargoSagaError
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}
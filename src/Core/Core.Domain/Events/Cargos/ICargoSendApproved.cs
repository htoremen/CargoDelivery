namespace Core.Domain.Events.Cargos;

public interface ICargoSendApproved : IEvent
{
    public Guid OrderId { get; set; }
}

public class CargoSendApproved : ICargoSendApproved
{
    public Guid CorrelationId { get; set; }
    public Guid OrderId { get; set; }
}
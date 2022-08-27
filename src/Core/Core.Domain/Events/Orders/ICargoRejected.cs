namespace Core.Domain.Events.Orders;

public interface ICargoRejected : IEvent
{
    public Guid OrderId { get; set; }
}

public class CargoRejected : ICargoRejected
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
}
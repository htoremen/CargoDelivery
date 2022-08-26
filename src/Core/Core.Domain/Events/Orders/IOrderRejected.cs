namespace Core.Domain.Events.Orders;

public interface IOrderRejected : IEvent
{
    public Guid OrderId { get; set; }
}

public class OrderRejected: IOrderRejected
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
}
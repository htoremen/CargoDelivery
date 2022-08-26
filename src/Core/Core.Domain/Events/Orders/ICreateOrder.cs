namespace Core.Domain.Events.Orders;

public interface ICreateOrder : IEvent
{
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}

public class CreateOrder : ICreateOrder
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
}

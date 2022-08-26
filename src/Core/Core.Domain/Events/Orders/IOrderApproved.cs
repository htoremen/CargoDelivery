namespace Core.Domain.Events.Orders;
public interface IOrderApproved : IEvent
{
    public Guid OrderId { get; set; }
}

public class OrderApproved : IOrderApproved
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
}
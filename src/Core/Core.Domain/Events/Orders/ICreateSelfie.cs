namespace Core.Domain.Events.Orders;

public interface ICreateSelfie : IEvent
{
    public Guid OrderId { get; set; }
}

public class CreateSelfie : ICreateSelfie
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
}
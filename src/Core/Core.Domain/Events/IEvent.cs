namespace Core.Domain.Events;

public interface IEvent : ICommand
{
    public Guid CorrelationId { get; set; }
}
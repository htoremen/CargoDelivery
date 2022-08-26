namespace Core.Domain.Events;

public interface IEvent : ICommand
{
    public Guid Id { get; set; }
}
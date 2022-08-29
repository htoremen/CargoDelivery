namespace Core.Domain.Events;

public interface IEvent : ICommand, CorrelatedBy<Guid>
{
}
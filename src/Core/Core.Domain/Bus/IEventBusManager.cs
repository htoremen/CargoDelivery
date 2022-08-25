namespace Core.Domain.Bus;

public interface IEventBusManager<TBus> where TBus : IBus
{
    Task Send<T>(T request, string queueName, CancellationToken cancellationToken = default(CancellationToken))
        where T : ICommand;

    Task Send<T>(T request, string queueName, string routingKey, CancellationToken cancellationToken = default(CancellationToken))
        where T : ICommand;
}
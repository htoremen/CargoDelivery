using Bus;

namespace Core.Application;

public class EventBusService<TBus> : IEventBusService<TBus> where TBus : IBus
{
    private readonly IEventBusManager<TBus> _eventBusManager;
    public EventBusService(IEventBusManager<TBus> eventBusManager)
    {
        _eventBusManager = eventBusManager;
    }

    public async Task<bool> SendCommandAsync<T>(T command, string queueName, CancellationToken cancellationToken) where T : ICommand
    {
        try
        {
            await _eventBusManager.Send(command, queueName, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> SendCommandAsync<T>(T command, string queueName, string routingKey, CancellationToken cancellationToken) where T : ICommand
    {
        try
        {
            await _eventBusManager.Send(command, queueName, routingKey, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
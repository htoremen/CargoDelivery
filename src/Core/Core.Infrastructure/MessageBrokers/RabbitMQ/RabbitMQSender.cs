using Core.Domain;
using Core.Domain.Enums;
using Core.Domain.MessageBrokers;
using MassTransit;

namespace Core.Infrastructure.MessageBrokers.RabbitMQ;

public class RabbitMQSender<T> : IMessageSender<T>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;

    public RabbitMQSender(ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration, IPublishEndpoint publishEndpoint)
    {
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendAsync(T message, MetaData metaData = null, CancellationToken cancellationToken = default)
    {
        await _sendEndpoint.Send(message, cancellationToken);
    }

    public async Task Publish(T message, MetaData metaData = null, CancellationToken cancellationToken = default)
    {
        await _publishEndpoint.Publish(message, cancellationToken);
    }
}
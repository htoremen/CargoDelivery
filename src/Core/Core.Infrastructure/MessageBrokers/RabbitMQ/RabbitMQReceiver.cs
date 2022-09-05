using Core.Domain;
using Core.Domain.Enums;
using Core.Domain.MessageBrokers;
using MassTransit;

namespace Core.Infrastructure.MessageBrokers.RabbitMQ;

public class RabbitMQReceiver<T> : IMessageReceiver<T>
{
    private readonly ISendEndpoint _sendEndpoint;
    private readonly IQueueConfiguration _queueConfiguration;
    private string _queueName;
    private readonly RabbitMQReceiverOptions _options;

    public RabbitMQReceiver(RabbitMQReceiverOptions options, ISendEndpointProvider sendEndpointProvider, IQueueConfiguration queueConfiguration)
    {
        _options = options;
        _queueConfiguration = queueConfiguration;
        _sendEndpoint = sendEndpointProvider.GetSendEndpoint(new($"queue:{_queueConfiguration.Names[QueueName.CargoSaga]}")).Result;
    }

    public void Receive(Action<T, MetaData> action)
    {
        throw new NotImplementedException();
    }
}
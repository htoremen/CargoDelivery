using Confluent.Kafka;
using Core.Domain;
using Core.Domain.Enums;
using Core.Domain.MessageBrokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Infrastructure.MessageBrokers.Kafka;

public class KafkaSender<T> : IMessageSender<T>
{
    private string _topic;
    private readonly IProducer<Null, string> _producer;
    private readonly IQueueConfiguration _queueConfiguration;

    public KafkaSender(IQueueConfiguration queueConfiguration)
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _queueConfiguration = queueConfiguration;
        _topic = _queueConfiguration.Names[QueueName.CargoSaga];
    }

    public async Task SendAsync(T message, MetaData metaData = null, CancellationToken cancellationToken = default)
    {
        _ = await _producer.ProduceAsync(_topic, new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(new Message<T>
            {
                Data = message,
                MetaData = metaData,
            }),
        }, cancellationToken);
    }
}
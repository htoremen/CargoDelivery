using Confluent.Kafka;
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
    private readonly string _topic;
    private readonly IProducer<Null, string> _producer;

    public KafkaSender(string bootstrapServers, string topic)
    {
        _topic = topic;

        var config = new ProducerConfig { BootstrapServers = bootstrapServers };
        _producer = new ProducerBuilder<Null, string>(config).Build();
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

using Core.Infrastructure.MessageBrokers.Kafka;
using Core.Infrastructure.MessageBrokers.RabbitMQ;

namespace Core.Infrastructure.MessageBrokers;

public class MessageBrokerOptions
{
    public string Provider { get; set; }
    public RabbitMQOptions RabbitMQ { get; set; }
    public KafkaOptions Kafka { get; set; }

    public bool UsedRabbitMQ()
    {
        return Provider == "RabbitMQ";
    }
    public bool UsedKafka()
    {
        return Provider == "Kafka";
    }
}


namespace Core.Infrastructure.MessageBrokers.Kafka;

public class KafkaOptions
{
    public string BootstrapServers { get; set; }
    public string SchemaRegistryUrl { get; set; }   
    public string SecurityProtocol { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string SaslMechanism { get; set; }

    public string GroupId { get; set; }

    public Dictionary<string, string> Topics { get; set; }
}
namespace Core.Infrastructure.MessageBrokers.RabbitMQ;

public class RabbitMQOptions
{
    public string Name { get; set; }
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
    public int RetryCount { get; set; }
    public int RetryTimeInterval { get; set; }
    public int PrefetchCount { get; set; }
    public int TrackingPeriod { get; set; }
    public int TripThreshold { get; set; }
    public int ActiveThreshold { get; set; }
    public int ResetInterval { get; set; }
    public string ConnectionString
    {
        get
        {
            return $"amqp://{UserName}:{Password}@{HostName}/%2f";
        }
    }
}
using Core.Infrastructure.MessageBrokers;
using Core.Infrastructure.Notification;
using Core.Infrastructure.Telemetry;
using NoSQLMongo.Infrastructure.Settings;

namespace Core.Infrastructure;

public class AppSettings
{
    public MessageBrokerOptions MessageBroker { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public MongoDbSettings MongoDbSettings { get; set; }

    public ServiceUrls ServiceUrls { get; set; }
    public CachingOptions Caching { get; set; }
    public NotificationOptions Notification { get; set; }    

    public Authenticate Authenticate { get; set; }
    public TelemetryOptions Telemetry { get; set; }
}

public class Authenticate
{
    public string Secret { get; set; }
    public string RefreshTokenTTL { get; set; }
}
public class ConnectionStrings
{
    public string ConnectionString { get; set; }
    public string Monitoring { get; set; }
}

public class ServiceUrls
{
    public string Saga { get; set; }
    public string Cargo { get; set; }
    public string Route { get; set; }
    public string Delivery { get; set; }
    public string Payment { get; set; }
}
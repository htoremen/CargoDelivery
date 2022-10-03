using Core.Infrastructure.MessageBrokers;
using NoSQLMongo.Infrastructure.Settings;

namespace Core.Infrastructure;

public class AppSettings
{
    public MessageBrokerOptions MessageBroker { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public MongoDbSettings MongoDbSettings { get; set; }

    public ServiceUrls ServiceUrls { get; set; }
    public CachingOptions Caching { get; set; }
}


public class ConnectionStrings
{
    public string SagaConnectionString { get; set; }
    public string CargoConnectionString { get; set; }
    public string RouteConnectionString { get; set; }
    public string DeliveryConnectionString { get; set; }
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
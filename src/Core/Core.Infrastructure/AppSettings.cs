using Core.Infrastructure.MessageBrokers;
using NoSQLMongo.Infrastructure.Settings;

namespace Core.Infrastructure;

public class AppSettings
{
    public MessageBrokerOptions MessageBroker { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
    public MongoDbSettings MongoDbSettings { get; set; }
}


public class ConnectionStrings
{
    public string SagaConnectionString { get; set; }
    public string CargoConnectionString { get; set; }
    public string RouteConnectionString { get; set; }
    public string DeliveryConnectionString { get; set; }
}
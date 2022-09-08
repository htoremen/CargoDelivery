using Core.Infrastructure.MessageBrokers;

namespace Core.Infrastructure;

public class AppSettings
{
    public MessageBrokerOptions MessageBroker { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
}


public class ConnectionStrings
{
    public string SagaConnectionString { get; set; }
    public string CargoConnectionString { get; set; }
    public string DeliveryConnectionString { get; set; }
}
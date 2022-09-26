using Core.Infrastructure;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Common.Extensions;

public static class GeneralExtensions
{
    public static Uri GetRabbitMqConnection(AppSettings appSettings)
    {
        var config = appSettings.MessageBroker.RabbitMQ;
        //ConnectionFactory factory = new ConnectionFactory
        //{
        //    UserName = appSettings.MessageBroker.RabbitMQ.UserName,
        //    Password = appSettings.MessageBroker.RabbitMQ.Password,
        //    VirtualHost = appSettings.MessageBroker.RabbitMQ.VirtualHost,
        //    HostName = appSettings.MessageBroker.RabbitMQ.HostName,
        //    Port = AmqpTcpEndpoint.UseDefaultPort
        //};

        //var connection = factory.CreateConnection();

        var connectionString = $"amqp://{config.Password}:{config.Password}@{config.HostName}{config.VirtualHost}";
        Uri uri = new Uri(connectionString, UriKind.Absolute);

        return uri;
    }
}

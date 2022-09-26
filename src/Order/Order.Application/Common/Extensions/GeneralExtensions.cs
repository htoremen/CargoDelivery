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
        ConnectionFactory factory = new ConnectionFactory
        {
            UserName = appSettings.MessageBroker.RabbitMQ.UserName,
            Password = appSettings.MessageBroker.RabbitMQ.Password,
            VirtualHost = appSettings.MessageBroker.RabbitMQ.VirtualHost,
            HostName = appSettings.MessageBroker.RabbitMQ.HostName,
            Port = AmqpTcpEndpoint.UseDefaultPort
        };
        return factory.Uri;
    }
}

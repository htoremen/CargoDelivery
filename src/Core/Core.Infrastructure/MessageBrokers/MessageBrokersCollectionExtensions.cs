using Core.Domain.MessageBrokers;
using Core.Infrastructure.MessageBrokers.Kafka;
using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure.MessageBrokers;

public static class MessageBrokersCollectionExtensions
{
    public static IServiceCollection AddRabbitMQSender<T>(this IServiceCollection services)
    {
        services.AddSingleton<IMessageSender<T>, RabbitMQSender<T>>();
        return services;
    }

    public static IServiceCollection AddKafkaSender<T>(this IServiceCollection services, KafkaOptions options)
    {
        services.AddSingleton<IMessageSender<T>, KafkaSender<T>>();
        return services;
    }

    public static IServiceCollection AddMessageBusSender<T>(this IServiceCollection services, MessageBrokerOptions options, IHealthChecksBuilder healthChecksBuilder = null, HashSet<string> checkDulicated = null)
    {
        if (options.UsedRabbitMQ())
        {
            services.AddRabbitMQSender<T>();            
        }
        else if (options.UsedKafka())
        {
            services.AddKafkaSender<T>(options.Kafka);
        }

        return services;
    }
}

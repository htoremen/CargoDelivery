using Core.Domain.MessageBrokers;
using Core.Infrastructure.MessageBrokers.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Core.Infrastructure.MessageBrokers;

public static class MessageBrokersCollectionExtensions
{
    public static IServiceCollection AddRabbitMQSender<T>(this IServiceCollection services)
    {
        services.AddSingleton<IMessageSender<T>, RabbitMQSender<T>>();
        return services;
    }

    public static IServiceCollection AddMessageBusSender<T>(this IServiceCollection services, MessageBrokerOptions options, IHealthChecksBuilder healthChecksBuilder = null, HashSet<string> checkDulicated = null)
    {
        if (options.UsedRabbitMQ())
        {
            services.AddRabbitMQSender<T>();

            if (healthChecksBuilder != null)
            {
                var name = "Message Broker (RabbitMQ)";

                if (checkDulicated == null || !checkDulicated.Contains(name))
                {
                    healthChecksBuilder.AddRabbitMQ(
                        rabbitConnectionString: options.RabbitMQ.ConnectionString,
                        name: name,
                        failureStatus: HealthStatus.Degraded);
                }

                checkDulicated?.Add(name);
            }
        }
        else if (options.UsedKafka())
        {
            //services.AddKafkaSender<T>(options.Kafka);

            //if (healthChecksBuilder != null)
            //{
            //    var name = "Message Broker (Kafka)";

            //    if (checkDulicated == null || !checkDulicated.Contains(name))
            //    {
            //        healthChecksBuilder.AddKafka(
            //            setup =>
            //            {
            //                setup.BootstrapServers = options.Kafka.BootstrapServers;
            //                setup.MessageTimeoutMs = 5000;
            //            },
            //            name: name,
            //            failureStatus: HealthStatus.Degraded);
            //    }

            //    checkDulicated?.Add(name);
            //}
        }

        return services;
    }
}

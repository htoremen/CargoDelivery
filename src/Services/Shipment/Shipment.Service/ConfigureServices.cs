using Core.Application.Common.Interfaces;
using Core.Domain;
using Core.Domain.Enums;
using Core.Infrastructure;
using MassTransit;
using MediatR;
using Shipment.Application.Consumer;
using Shipment.Service.Services;

namespace Shipment.Service;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        return services;
    }

    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddHealthChecks();
        return services;
    }
    public static IServiceCollection AddStaticValues(this IServiceCollection services, AppSettings appSettings)
    {
        var rabbitMQ = appSettings.MessageBroker.RabbitMQ;
        RabbitMQStaticValues.ResetInterval = rabbitMQ.ResetInterval;
        RabbitMQStaticValues.RetryTimeInterval = rabbitMQ.RetryTimeInterval;
        RabbitMQStaticValues.RetryCount = rabbitMQ.RetryCount;
        RabbitMQStaticValues.PrefetchCount = rabbitMQ.PrefetchCount;
        RabbitMQStaticValues.TrackingPeriod = rabbitMQ.TrackingPeriod;
        RabbitMQStaticValues.ActiveThreshold = rabbitMQ.ActiveThreshold;

        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);
        var messageBroker = appSettings.MessageBroker;

        services.AddMassTransit(x => { UsingRabbitMq(x, messageBroker, queueConfiguration); });
        services.ConfigureMassTransitHostOptions(messageBroker);

        return services;
    }

    private static void UsingRabbitMq(IBusRegistrationConfigurator x, Core.Infrastructure.MessageBrokers.MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.SetSnakeCaseEndpointNameFormatter();

        x.AddConsumer<ShipmentReceivedConsumer, ShipmentReceivedConsumerDefinition>();
        x.AddConsumer<StartDistributionConsumer, StartDistributionConsumerDefinition>();

        var config = messageBroker.RabbitMQ;
        x.UsingRabbitMq((context, cfg) =>
        {
            var mediator = context.GetRequiredService<IMediator>();
            cfg.UseJsonSerializer();
            cfg.Host(config.HostName, config.VirtualHost, h =>
            {
                h.Username(config.UserName);
                h.Password(config.Password);
            });

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.ShipmentReceived], e => { e.ConfigureConsumer<ShipmentReceivedConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.StartDistribution], e => { e.ConfigureConsumer<StartDistributionConsumer>(context); });

            cfg.ConfigureEndpoints(context);
        });
    }
}
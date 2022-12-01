using Core.Domain;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using Core.Infrastructure.MessageBrokers;
using MassTransit;
using MediatR;
using Payment.Application.Consumer;
using Payment.Infrastructure.Healths;
using Payment.Service.Services;

namespace Payment.Service;

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
            services.AddHealthChecks()
                .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings))
                .AddCheck<DeliveryHealthCheck>("delivery-grpc-server");   
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

    private static void UsingRabbitMq(IBusRegistrationConfigurator x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.SetSnakeCaseEndpointNameFormatter();

        x.AddConsumer<CardPaymentConsumer, CardPaymentConsumerDefinition>();
        x.AddConsumer<FreeDeliveryConsumer, FreeDeliveryConsumerDefinition>();
        x.AddConsumer<PayAtDoorConsumer, PayAtDoorConsumerDefinition>();

        var config = messageBroker.RabbitMQ;
        x.UsingRabbitMq((context, cfg) =>
        {
            var mediator = context.GetRequiredService<IMediator>();
            cfg.Host(config.HostName, config.VirtualHost, h =>
            {
                h.Username(config.UserName);
                h.Password(config.Password);
            });

            cfg.UseJsonSerializer();

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CardPayment], e => { e.ConfigureConsumer<CardPaymentConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.FreeDelivery], e => { e.ConfigureConsumer<FreeDeliveryConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.PayAtDoor], e => { e.ConfigureConsumer<PayAtDoorConsumer>(context); });

            cfg.ConfigureEndpoints(context);
        });
    }
}
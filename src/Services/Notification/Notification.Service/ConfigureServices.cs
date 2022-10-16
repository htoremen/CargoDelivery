using Core.Application.Common.Interfaces;
using Core.Domain;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using MassTransit;
using MediatR;
using Notification.Application.Consumer;
using Notification.Service.Services;

namespace Notification.Service;

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
            .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings));
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

        x.AddConsumer<SendMailConsumer, SendMailConsumerDefinition>();
        x.AddConsumer<SendSmsConsumer, SendSmsConsumerDefinition>();
        x.AddConsumer<PushNotificationConsumer, PushNotificationConsumerDefinition>();

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

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.SendMail], e => { e.ConfigureConsumer<SendMailConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.SendSms], e => { e.ConfigureConsumer<SendSmsConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.PushNotification], e => { e.ConfigureConsumer<PushNotificationConsumer>(context); });

            cfg.ConfigureEndpoints(context);
        });
    }
}
using Core.Domain;
using MassTransit;
using MediatR;
using Cargo.Service.Services;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using Cargo.GRPC.Server.Services;
using Core.Infrastructure.Common.Extensions;

namespace Cargo.Service;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        return services;
    }

    public static WebApplication MapGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<DebitService>();
        app.MapGrpcService<CargoService>();
        app.MapGrpcService<CargoHealthService>();
        return app;
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

        StaticValues.ConnectionString = appSettings.ConnectionStrings.ConnectionString;

        return services;
    }

    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddHealthChecks()
            .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings));

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

        x.AddConsumer<CreateDebitConsumer, CreateDebitConsumerDefinition>();
        x.AddConsumer<SendSelfieConsumer, SendSelfieConsumerDefinition>();
        x.AddConsumer<DebitApprovalConsumer, DebitApprovalConsumerDefinition>();
        x.AddConsumer<DebitRejectedConsumer, DebitRejectedConsumerDefinition>();

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

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CreateDebit], e => { e.ConfigureConsumer<CreateDebitConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.SendSelfie], e => { e.ConfigureConsumer<SendSelfieConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.DebitApproval], e => { e.ConfigureConsumer<DebitApprovalConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.DebitRejected], e => { e.ConfigureConsumer<DebitRejectedConsumer>(context); });

            cfg.ConfigureEndpoints(context);
        });
    }
}
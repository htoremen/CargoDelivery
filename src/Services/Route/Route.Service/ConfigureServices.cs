using Core.Domain;
using MassTransit;
using MediatR;
using Core.Domain.Enums;
using Route.Service.Services;
using Core.Infrastructure;
using Route.GRPC.Server.Services;
using Core.Infrastructure.Common.Extensions;
using Route.Infrastructure.Healths;
using Core.Application.Common.Interfaces;

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
        app.MapGrpcService<RouteService>();
        app.MapGrpcService<RouteHealthService>();
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

        return services;
    }

    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddHealthChecks()
            .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings))
            .AddCheck<CargoHealthCheck>("cargo-grpc-server");
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

        x.AddConsumer<ManuelRouteConsumer, ManuelRouteConsumerDefinition>();
        x.AddConsumer<AutoRouteConsumer, AutoRouteConsumerDefinition>();
        x.AddConsumer<StartRouteConsumer, StartRouteConsumerDefinition>();

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
            cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));
            cfg.ConfigureEndpoints(context);

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.ManuelRoute], e => { e.ConfigureConsumer<ManuelRouteConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.AutoRoute], e => { e.ConfigureConsumer<AutoRouteConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.StartRoute], e => { e.ConfigureConsumer<StartRouteConsumer>(context); });
        });
    }
}
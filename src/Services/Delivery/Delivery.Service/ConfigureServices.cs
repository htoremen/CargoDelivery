using Core.Domain;
using MassTransit;
using MediatR;
using Core.Domain.Enums;
using Route.Service.Services;
using Delivery.Application.Consumer;
using Core.Infrastructure;
using Delivery.GRPC.Server.Services;
using Core.Infrastructure.Common.Extensions;
using Delivery.Infrastructure.Healths;
using Core.Infrastructure.MessageBrokers;

namespace Delivery.Service;

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
        app.MapGrpcService<DeliveryService>();
        app.MapGrpcService<DeliveryHealthService>();
        app.MapGrpcHealthChecksService();
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
        var messageBroker = appSettings.MessageBroker;
        if (messageBroker.UsedRabbitMQ())
        {
            services.AddHealthChecks()
                .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings));
        }

        //services.AddGrpcHealthChecks(o =>
        //{
        //    o.Services.MapService("RouteGrpc", r => r.Tags.Contains("GetRoute"));
        //}).AddCheck("RouteGrpc", () => HealthCheckResult.Healthy());

        services.AddHealthChecks()
            .AddCheck<CargoHealthCheck>("cargo-grpc-server")
            .AddCheck<RouteHealthCheck>("route-grpc-server");

        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);
        var messageBroker = appSettings.MessageBroker;

        services.AddMassTransit<IBus>(x => { UsingRabbitMq(x, messageBroker, queueConfiguration); });
        services.ConfigureMassTransitHostOptions(messageBroker);

        return services;
    }

    private static void UsingRabbitMq(IBusRegistrationConfigurator<IBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queue)
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.SetSnakeCaseEndpointNameFormatter();

        x.AddConsumer<StartDeliveryConsumer, StartDeliveryConsumerDefinition>();
        x.AddConsumer<NewDeliveryConsumer, NewDeliveryConsumerDefinition>();
        x.AddConsumer<VerificationCodeConsumer, VerificationCodeConsumerefinition>();

        x.AddConsumer<CreateDeliveryConsumer, CreateDeliveryConsumerDefinition>();
        x.AddConsumer<NotDeliveredConsumer, NotDeliveredConsumerDefinition>();
        x.AddConsumer<CreateRefundConsumer, CreateRefundConsumerDefinition>();

        x.AddConsumer<DeliveryCompletedConsumer, DeliveryCompletedConsumerDefinition>();
        x.AddConsumer<ShiftCompletionConsumer, ShiftCompletionConsumerDefinition>();

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

            cfg.ReceiveEndpoint(queue.Names[QueueName.StartDelivery], e => { e.ConfigureConsumer<StartDeliveryConsumer>(context); });
            cfg.ReceiveEndpoint(queue.Names[QueueName.NewDelivery], e => { e.ConfigureConsumer<NewDeliveryConsumer>(context); });
            cfg.ReceiveEndpoint(queue.Names[QueueName.VerificationCode], e => { e.ConfigureConsumer<VerificationCodeConsumer>(context); });

            cfg.ReceiveEndpoint(queue.Names[QueueName.CreateDelivery], e => { e.ConfigureConsumer<CreateDeliveryConsumer>(context); });
            cfg.ReceiveEndpoint(queue.Names[QueueName.NotDelivered], e => { e.ConfigureConsumer<NotDeliveredConsumer>(context); });
            cfg.ReceiveEndpoint(queue.Names[QueueName.CreateRefund], e => { e.ConfigureConsumer<CreateRefundConsumer>(context); });
            cfg.ReceiveEndpoint(queue.Names[QueueName.DeliveryCompleted], e => { e.ConfigureConsumer<DeliveryCompletedConsumer>(context); });

            cfg.ReceiveEndpoint(queue.Names[QueueName.ShiftCompletion], e => { e.ConfigureConsumer<ShiftCompletionConsumer>(context); });

            cfg.ConfigureEndpoints(context);
        });
    }
}
using Core.Domain;
using Core.Domain.Bus;
using MassTransit;
using MediatR;
using Core.Domain.Enums;
using Route.Service.Services;
using Delivery.Application.Consumer;
using Core.Infrastructure;
using Delivery.GRPC.Client.Services;
using Delivery.GRPC.Server.Services;
using Core.Infrastructure.Common.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Delivery.Infrastructure.Healths;
using Core.Infrastructure.MessageBrokers;
using Deliveries;
using Confluent.Kafka;

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

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        var bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(messageBroker.RabbitMQ.HostName, messageBroker.RabbitMQ.VirtualHost, h =>
            {
                h.Username(messageBroker.RabbitMQ.UserName);
                h.Password(messageBroker.RabbitMQ.Password);
            });
        });

        services.AddSingleton<IPublishEndpoint>(bus);
        services.AddSingleton<ISendEndpointProvider>(bus);
        services.AddSingleton<IBus>(bus);
        services.AddSingleton<IBusControl>(bus);

        return services;
    }

    private static void UsingRabbitMq(IBusRegistrationConfigurator<IBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.AddConsumer<StartDeliveryConsumer, StartDeliveryConsumerDefinition>();
        x.AddConsumer<NewDeliveryConsumer, NewDeliveryConsumerDefinition>();
        x.AddConsumer<CreateDeliveryConsumer, CreateDeliveryConsumerDefinition>();
        x.AddConsumer<NotDeliveredConsumer, NotDeliveredConsumerDefinition>();
        x.AddConsumer<CreateRefundConsumer, CreateRefundConsumerDefinition>();
        x.AddConsumer<DeliveryCompletedConsumer, DeliveryCompletedConsumerDefinition>();
        //  x.AddConsumer<ShiftCompletionConsumer, ShiftCompletionConsumerDefinition>();

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
            cfg.ConfigureEndpoints(context);

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.StartDelivery], e => { e.ConfigureConsumer<StartDeliveryConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.NewDelivery], e => { e.ConfigureConsumer<NewDeliveryConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CreateDelivery], e => { e.ConfigureConsumer<CreateDeliveryConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.NotDelivered], e => { e.ConfigureConsumer<NotDeliveredConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CreateRefund], e => { e.ConfigureConsumer<CreateRefundConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.DeliveryCompleted], e => { e.ConfigureConsumer<DeliveryCompletedConsumer>(context); });
        });
    }
}
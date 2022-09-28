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

        services.AddMassTransit<IBus>(x =>
        {
            x.AddConsumer<StartDeliveryConsumer>();
            x.AddConsumer<NewDeliveryConsumer>();
            x.AddConsumer<CreateDeliveryConsumer>();
            x.AddConsumer<NotDeliveredConsumer>();
            x.AddConsumer<CreateRefundConsumer>();
            x.AddConsumer<DeliveryCompletedConsumer>();

            x.SetKebabCaseEndpointNameFormatter();
            if (messageBroker.UsedRabbitMQ())
                UsingRabbitMq(x, messageBroker, queueConfiguration);
            else if (messageBroker.UsedKafka())
                UsingKafka(x, messageBroker, queueConfiguration);            

        });

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        if (messageBroker.UsedRabbitMQ())
        {
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
        }
        return services;
    }

    private static void UsingKafka(IBusRegistrationConfigurator<IBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        var config = messageBroker.Kafka;
        x.AddRider(rider =>
        {
            rider.UsingKafka((context, k) =>
            {
                var mediator = context.GetRequiredService<IMediator>();
                k.Host(config.BootstrapServers);

                k.TopicEndpoint<IStartDelivery>(queueConfiguration.Names[QueueName.StartDelivery], config.GroupId, e =>
                {
                    e.ConfigureConsumer<StartDeliveryConsumer>(context);
                });

                k.TopicEndpoint<INewDelivery>(queueConfiguration.Names[QueueName.NewDelivery], config.GroupId, e =>
                {
                    e.ConfigureConsumer<NewDeliveryConsumer>(context);
                });

                k.TopicEndpoint<ICreateDelivery>(queueConfiguration.Names[QueueName.CreateDelivery], config.GroupId, e =>
                {
                    e.ConfigureConsumer<CreateDeliveryConsumer>(context);
                });

                k.TopicEndpoint<INotDelivered>(queueConfiguration.Names[QueueName.NotDelivered], config.GroupId, e =>
                {
                    e.ConfigureConsumer<NotDeliveredConsumer>(context);
                });

                k.TopicEndpoint<ICreateRefund>(queueConfiguration.Names[QueueName.CreateRefund], config.GroupId, e =>
                {
                    e.ConfigureConsumer<CreateRefundConsumer>(context);
                });

                k.TopicEndpoint<IDeliveryCompleted>(queueConfiguration.Names[QueueName.DeliveryCompleted], config.GroupId, e =>
                {
                    e.ConfigureConsumer<DeliveryCompletedConsumer>(context);
                });
            });
        });
    }

    private static void UsingRabbitMq(IBusRegistrationConfigurator<IBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
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

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.StartDelivery], e =>
            {
                e.PrefetchCount = 1;
                e.UseMessageRetry(x => x.Interval(config.RetryCount, config.ResetInterval));
                e.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(config.TrackingPeriod);
                    cb.TripThreshold = config.TripThreshold;
                    cb.ActiveThreshold = config.ActiveThreshold;
                    cb.ResetInterval = TimeSpan.FromMinutes(config.ResetInterval);
                });
                e.ConfigureConsumer<StartDeliveryConsumer>(context);
            });

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.NewDelivery], e =>
            {
                e.PrefetchCount = 1;
                e.UseMessageRetry(x => x.Interval(config.RetryCount, config.ResetInterval));
                e.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(config.TrackingPeriod);
                    cb.TripThreshold = config.TripThreshold;
                    cb.ActiveThreshold = config.ActiveThreshold;
                    cb.ResetInterval = TimeSpan.FromMinutes(config.ResetInterval);
                });
                e.ConfigureConsumer<NewDeliveryConsumer>(context);
            });

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CreateDelivery], e =>
            {
                e.PrefetchCount = 1;
                e.UseMessageRetry(x => x.Interval(config.RetryCount, config.ResetInterval));
                e.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(config.TrackingPeriod);
                    cb.TripThreshold = config.TripThreshold;
                    cb.ActiveThreshold = config.ActiveThreshold;
                    cb.ResetInterval = TimeSpan.FromMinutes(config.ResetInterval);
                });
                e.ConfigureConsumer<CreateDeliveryConsumer>(context);
            });

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.NotDelivered], e =>
            {
                e.PrefetchCount = 1;
                e.UseMessageRetry(x => x.Interval(config.RetryCount, config.ResetInterval));
                e.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(config.TrackingPeriod);
                    cb.TripThreshold = config.TripThreshold;
                    cb.ActiveThreshold = config.ActiveThreshold;
                    cb.ResetInterval = TimeSpan.FromMinutes(config.ResetInterval);
                });
                e.ConfigureConsumer<NotDeliveredConsumer>(context);
            });

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CreateRefund], e =>
            {
                e.PrefetchCount = 1;
                e.UseMessageRetry(x => x.Interval(config.RetryCount, config.ResetInterval));
                e.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(config.TrackingPeriod);
                    cb.TripThreshold = config.TripThreshold;
                    cb.ActiveThreshold = config.ActiveThreshold;
                    cb.ResetInterval = TimeSpan.FromMinutes(config.ResetInterval);
                });
                e.ConfigureConsumer<CreateRefundConsumer>(context);
            });

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.DeliveryCompleted], e =>
            {
                e.PrefetchCount = 1;
                e.UseMessageRetry(x => x.Interval(config.RetryCount, config.ResetInterval));
                e.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(config.TrackingPeriod);
                    cb.TripThreshold = config.TripThreshold;
                    cb.ActiveThreshold = config.ActiveThreshold;
                    cb.ResetInterval = TimeSpan.FromMinutes(config.ResetInterval);
                });
                e.ConfigureConsumer<DeliveryCompletedConsumer>(context);
            });


        });
    }
}
﻿using Core.Domain;
using Core.Domain.Bus;
using MassTransit;
using MediatR;
using Cargo.Service.Services;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using Cargos;
using Cargo.GRPC.Server.Services;
using Core.Infrastructure.Common.Extensions;
using Confluent.Kafka;
using Core.Infrastructure.Telemetry.Options;
using Cargo.Application.Telemetry;
using System.Diagnostics;

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
        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);
        var messageBroker = appSettings.MessageBroker;

        services.AddMassTransit<IEventBus>(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.SetSnakeCaseEndpointNameFormatter();

            x.AddConsumer<CreateDebitConsumer>(typeof(CreateDebitConsumerDefinition));
            // x.AddConsumer<CreateCargoConsumer>();
            x.AddConsumer<SendSelfieConsumer>();
            x.AddConsumer<CargoApprovalConsumer>();
            x.AddConsumer<CargoRejectedConsumer>();
            // x.AddConsumer<CreateDebitHistoryConsumer>();
           // x.AddConsumer<CreateDebitFaultConsumer>();

            UsingRabbitMq(x, messageBroker, queueConfiguration);

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
        else if (messageBroker.UsedRabbitMQ())
        {

        }

        return services;
    }
    private static void UsingRabbitMq(IBusRegistrationConfigurator<IEventBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
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

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CreateDebit], e => { e.ConfigureConsumer<CreateDebitConsumer>(context); });

            

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.SendSelfie], e =>
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
                e.ConfigureConsumer<SendSelfieConsumer>(context);
            });

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoApproval], e =>
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
                e.ConfigureConsumer<CargoApprovalConsumer>(context);
            });

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoRejected], e =>
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
                e.ConfigureConsumer<CargoRejectedConsumer>(context);
            });

            cfg.ConfigureEndpoints(context);

        });
    }



    private static void UsingKafka(IBusRegistrationConfigurator<IEventBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        var config = messageBroker.Kafka;
        x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance));

        x.AddRider(rider =>
        {
            rider.AddConsumer<CreateDebitConsumer>();
            //rider.AddConsumer<SendSelfieConsumer>();
            //rider.AddConsumer<CargoApprovalConsumer>();
            //rider.AddConsumer<CargoRejectedConsumer>();

            rider.AddConsumersFromNamespaceContaining<CreateDebitConsumer>();
            //rider.AddConsumersFromNamespaceContaining<SendSelfieConsumer>();
            //rider.AddConsumersFromNamespaceContaining<CargoApprovalConsumer>();
            //rider.AddConsumersFromNamespaceContaining<CargoRejectedConsumer>();

            rider.UsingKafka((context, k) =>
            {
                var mediator = context.GetRequiredService<IMediator>();
                k.Host(config.BootstrapServers);

                k.TopicEndpoint<string, ICreateDebit>(queueConfiguration.Names[QueueName.CreateDebit], config.GroupId, e =>
                {
                    e.AutoOffsetReset = AutoOffsetReset.Earliest;
                    e.ConfigureConsumer<CreateDebitConsumer>(context);
                });

                //k.TopicEndpoint<string, ISendSelfie>(queueConfiguration.Names[QueueName.SendSelfie], config.GroupId, e =>
                //{
                //    e.AutoOffsetReset = AutoOffsetReset.Earliest;
                //    e.ConfigureConsumer<SendSelfieConsumer>(context);
                //});

                //k.TopicEndpoint<string, ICargoApproval>(queueConfiguration.Names[QueueName.CargoApproval], config.GroupId, e =>
                //{
                //    e.AutoOffsetReset = AutoOffsetReset.Earliest;
                //    e.ConfigureConsumer<CargoApprovalConsumer>(context);
                //});

                //k.TopicEndpoint<string, ICargoRejected>(queueConfiguration.Names[QueueName.CargoRejected], config.GroupId, e =>
                //{
                //    e.AutoOffsetReset = AutoOffsetReset.Earliest;
                //    e.ConfigureConsumer<CargoRejectedConsumer>(context);
                //});

            });
        });
    }

}
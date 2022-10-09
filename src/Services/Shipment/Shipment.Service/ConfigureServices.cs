﻿using Core.Application.Common.Interfaces;
using Core.Domain;
using Core.Domain.Bus;
using Core.Infrastructure;
using MassTransit;
using MediatR;
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


    public static IServiceCollection AddEventBus(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);
        var messageBroker = appSettings.MessageBroker;

        services.AddMassTransit<IEventBus>(x =>
        {
          //  x.AddConsumer<CardPaymentConsumer>();

            x.SetKebabCaseEndpointNameFormatter();
            if (messageBroker.UsedRabbitMQ())
                UsingRabbitMq(x, messageBroker, queueConfiguration);
            
        });


        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());


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

    private static void UsingRabbitMq(IBusRegistrationConfigurator<IEventBus> x, Core.Infrastructure.MessageBrokers.MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
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

            //cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CardPayment], e =>
            //{
            //    e.PrefetchCount = 1;
            //    e.UseMessageRetry(x => x.Interval(config.RetryCount, config.ResetInterval));
            //    e.UseCircuitBreaker(cb =>
            //    {
            //        cb.TrackingPeriod = TimeSpan.FromMinutes(config.TrackingPeriod);
            //        cb.TripThreshold = config.TripThreshold;
            //        cb.ActiveThreshold = config.ActiveThreshold;
            //        cb.ResetInterval = TimeSpan.FromMinutes(config.ResetInterval);
            //    });
            //    e.ConfigureConsumer<CardPaymentConsumer>(context);
            //});

        });
    }
}
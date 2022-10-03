using Cargos;
using Core.Domain;
using Core.Domain.Bus;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using Core.Infrastructure.MessageBrokers;
using Deliveries;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Order.API.Services;
using System.Reflection;

namespace Order.API;

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
        var messageBroker = appSettings.MessageBroker;
        if (messageBroker.UsedRabbitMQ())
        {
            services.AddHealthChecks()
                .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings))
                //.AddUrlGroup(new Uri("https://localhost:5010/health"), "Saga.Service", HealthStatus.Degraded)
                //.AddUrlGroup(new Uri("https://localhost:5011/health"), "Cargo.Service", HealthStatus.Degraded)
                //.AddUrlGroup(new Uri("https://localhost:5012/health"), "Route.Service", HealthStatus.Degraded)
                //.AddUrlGroup(new Uri("https://localhost:5013/health"), "Delivery.Service", HealthStatus.Degraded)
                ;
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

            if (messageBroker.UsedRabbitMQ())
                UsingRabbitMq(x, messageBroker, queueConfiguration);
            else if (messageBroker.UsedKafka())
                UsingKafka(x, messageBroker, queueConfiguration);
            
        });

        if (messageBroker.UsedRabbitMQ())
        {
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
        }
        else if (messageBroker.UsedKafka())
        {

            //using var provider = services.BuildServiceProvider(true);
            //var busControl = provider.GetRequiredService<IBusControl>();
            //var startTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            //busControl.StartAsync(startTokenSource);
        }
        return services;
    }

    private static void UsingKafka(IBusRegistrationConfigurator<IEventBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        var config = messageBroker.Kafka;
        x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance));
        x.AddRider(rider =>
        {
            rider.UsingKafka((context, k) =>
            {
                var mediator = context.GetRequiredService<IMediator>();
                k.Host(config.BootstrapServers);
            });
        }); 
        
        x.AddOptions<MassTransitHostOptions>().Configure(options =>
        {
            options.WaitUntilStarted = true;
        });
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
            cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));
            cfg.UseMessageRetry(r => r.Immediate(5));

            cfg.ConfigureEndpoints(context);
        });
    }
}
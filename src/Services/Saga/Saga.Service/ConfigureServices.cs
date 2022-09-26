using Confluent.Kafka;
using Core.Application;
using Core.Domain;
using Core.Domain.Bus;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using Core.Infrastructure.MessageBrokers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Saga.Domain.Instances;
using Saga.Infrastructure.Persistence;
using Saga.Service.StateMachines;

namespace Saga.Service;
public static class ConfigureServices
{
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

    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfigurationRoot configuration, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);

        var messageBroker = appSettings.MessageBroker;

        services.AddMassTransit<IEventBus>(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            if (messageBroker.UsedRabbitMQ())
                CreateUsingRabbitMq(x, messageBroker, queueConfiguration);

            x.AddSagaStateMachine<CargoStateMachine, CargoStateInstance>()
                .EntityFrameworkRepository(config =>
                {
                    config.AddDbContext<DbContext, CargoStateDbContext>((p, b) =>
                    {
                        b.UseSqlServer(configuration.GetConnectionString("CargoStateDb"));
                    });
                });
        });

        //services.AddSingleton(rabbitMQConfig);
        //services.AddTransient(typeof(IEventBusService<>), typeof(EventBusService<>));
        //services.AddTransient(typeof(IEventBusManager<>), typeof(EventBusManager<>));

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });
        return services;
    }

    private static void CreateUsingRabbitMq(IBusRegistrationConfigurator<IEventBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        var config = messageBroker.RabbitMQ;
        x.AddBus(factory => MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(config.HostName, config.VirtualHost, h =>
            {
                h.Username(config.UserName);
                h.Password(config.Password);
            });

            cfg.UseJsonSerializer();
            cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoSaga], e =>
            {
                e.ConfigureSaga<CargoStateInstance>(factory);
            });
        }));
    }
}
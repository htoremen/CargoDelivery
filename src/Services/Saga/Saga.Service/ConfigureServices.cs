using Core.Domain;
using Core.Domain.Bus;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using Core.Infrastructure.MessageBrokers;
using Microsoft.EntityFrameworkCore;
using Saga.Domain.Instances;
using Saga.Infrastructure.Persistence;
using Saga.Service.StateMachines;
using MassTransit;
using Saga.Service.Components;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Core.Infrastructure.Common.AvroSerializers;
using System.Reflection;

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

    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfigurationRoot configuration, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);

        var messageBroker = appSettings.MessageBroker;
        services.AddMassTransit<IEventBus>(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            UsingRabbitMq(x, appSettings, queueConfiguration, configuration);
        });

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        return services;
    }

    private static void UsingRabbitMq(IBusRegistrationConfigurator<IEventBus> x, AppSettings appSettings, IQueueConfiguration queueConfiguration, IConfigurationRoot configuration)
    {
        var config = appSettings.MessageBroker.RabbitMQ;

        x.AddSagaStateMachine<CargoStateMachine, CargoStateInstance, SagaStateDefinition>()
            .EntityFrameworkRepository(config =>
            {
                config.AddDbContext<DbContext, CargoStateDbContext>((p, b) =>
                {
                    b.UseSqlServer(appSettings.ConnectionStrings.ConnectionString);
                });
            });
        x.AddSagas(Assembly.GetExecutingAssembly());
        x.AddSagasFromNamespaceContaining<CargoStateInstance>();
        x.AddSagasFromNamespaceContaining(typeof(CargoStateInstance));

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
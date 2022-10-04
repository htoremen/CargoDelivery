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
        if (messageBroker.UsedRabbitMQ())
        {
            services.AddMassTransit<IEventBus>(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                UsingRabbitMq(x, messageBroker, queueConfiguration, configuration);
            });
        }
        else if (messageBroker.UsedKafka())
        {
            var schemaRegistryConfig = new SchemaRegistryConfig { Url = messageBroker.Kafka.SchemaRegistryUrl };
            var schemaRegistryClient = new CachedSchemaRegistryClient(schemaRegistryConfig);
            services.AddSingleton<ISchemaRegistryClient>(schemaRegistryClient);

            services.AddMassTransit<IEventBus>(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                UsingKafka(x, messageBroker, queueConfiguration);
            });
        }

        //services.AddSingleton(rabbitMQConfig);
        //services.AddTransient(typeof(IEventBusService<>), typeof(EventBusService<>));
        //services.AddTransient(typeof(IEventBusManager<>), typeof(EventBusManager<>));

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        if (messageBroker.UsedKafka())
        {
            // var bus = MassTransit.Bus.Factory.CreateUsingkafka();

            //var provider = services.BuildServiceProvider();
            //var busControl = provider.GetRequiredService<IBusControl>(); 
            //var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            //busControl.StartAsync(cancellationToken);
        }
        return services;
    }
    private static void UsingKafka(IBusRegistrationConfigurator<IEventBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        var config = messageBroker.Kafka;

        x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance));
        x.AddSagaStateMachine<CargoStateMachine, CargoStateInstance, SagaStateDefinition>()
            .InMemoryRepository();
        x.AddRider(rider =>
        {
            rider.UsingKafka((context, k) =>
            {
                k.Host(config.BootstrapServers);

                k.TopicEndpoint<string>(queueConfiguration.Names[QueueName.CargoSaga], config.GroupId, e =>
                {
                    e.AutoOffsetReset = AutoOffsetReset.Earliest;

                    //var deserializier = new CustomAvroDeserializer<Avro.Specific>();
                  //  e.SetKeyDeserializer(new CustomAvroDeserializer<byte>().Deserialize(config.SchemaRegistryUrl, null).AsSyncOverAsync());

                    e.CreateIfMissing(t => t.NumPartitions = 1);
                });

                
            });

        });
    }

    private static void UsingRabbitMq(IBusRegistrationConfigurator<IEventBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration, IConfigurationRoot configuration)
    {
        var config = messageBroker.RabbitMQ;

        x.AddSagaStateMachine<CargoStateMachine, CargoStateInstance, SagaStateDefinition>()
            .EntityFrameworkRepository(config =>
            {
                config.AddDbContext<DbContext, CargoStateDbContext>((p, b) =>
                {
                    b.UseSqlServer(configuration.GetConnectionString("CargoStateDb"));
                });
            });
        //x.AddBus(factory => MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
        //{
        //    cfg.Host(config.HostName, config.VirtualHost, h =>
        //    {
        //        h.Username(config.UserName);
        //        h.Password(config.Password);
        //    });

        //    cfg.UseJsonSerializer();
        //    cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));

        //    cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoSaga], e =>
        //    {
        //        e.ConfigureSaga<CargoStateInstance>(factory);
        //    });
        //}));

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(config.HostName, config.VirtualHost, h =>
            {
                h.Username(config.UserName);
                h.Password(config.Password);
            });

            cfg.UseJsonSerializer();
           // cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoSaga], e =>
            {
                e.ConfigureSaga<CargoStateInstance>(context);
            });

            cfg.ConfigureEndpoints(context);
        });

    }
}
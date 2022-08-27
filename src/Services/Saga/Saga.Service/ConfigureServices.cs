using Core.Application;
using Core.Domain;
using Core.Domain.Bus;
using Core.Domain.Enums;
using MassTransit;
using Saga.Domain.Instances;

namespace Saga.Service;
public static class ConfigureServices
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);

        var rabbitMQConfig = new List<RabbitMqSettings>();
        var rabbitMqConfigurations = configuration.GetSection("RabbitMqSettings").Get<List<RabbitMqSettings>>();

        var config = rabbitMqConfigurations.FirstOrDefault(y => y.Name == "MainHost");
        if (config == null) throw new ArgumentNullException("MainHost section hasn't been found in the appsettings.");


        services.AddMassTransit<IEventBus>(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            //x.UsingRabbitMq((context, cfg) =>
            //{
            //    var mediator = context.GetRequiredService<IMediator>();
            //    cfg.Host(config.RabbitMqHostUrl, config.VirtualHost, h =>
            //    {
            //        h.Username(config.Username);
            //        h.Password(config.Password);
            //    });

            //    cfg.UseJsonSerializer();
            //    cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));
            //    cfg.ConfigureEndpoints(context);
            //});

            x.AddBus(factory => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(config.RabbitMqHostUrl);
                cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.SagaQueue], e =>
                {
                    e.ConfigureSaga<CargoStateInstance>(factory);
                });
            }));

        });

        services.AddSingleton(rabbitMQConfig);
        services.AddTransient(typeof(IEventBusService<>), typeof(EventBusService<>));
        services.AddTransient(typeof(IEventBusManager<>), typeof(EventBusManager<>));

        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });
        return services;
    }
}
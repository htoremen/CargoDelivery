using Core.Application;
using Core.Domain;
using Core.Domain.Bus;
using Core.Domain.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Saga.Domain.Instances;
using Saga.Infrastructure.Persistence;
using Saga.Service.StateMachines;

namespace Saga.Service;
public static class ConfigureServices
{
    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);

       // var rabbitMQConfig = new List<RabbitMqSettings>();
        var rabbitMqConfigurations = configuration.GetSection("RabbitMqSettings").Get<List<RabbitMqSettings>>();

        var config = rabbitMqConfigurations.FirstOrDefault(y => y.Name == "MainHost");
        if (config == null) throw new ArgumentNullException("MainHost section hasn't been found in the appsettings.");


        services.AddMassTransit<IEventBus>(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddBus(factory => MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(config.RabbitMqHostUrl);
                cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoSaga], e =>
                {
                    e.ConfigureSaga<CargoStateInstance>(factory);
                });
            }));

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
}
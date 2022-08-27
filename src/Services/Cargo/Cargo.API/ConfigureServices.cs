using Core.Domain;
using Core.Domain.Bus;
using MassTransit;
using MediatR;
using Cargo.API.Services;
using Core.Application;

namespace Cargo.API;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        return services;
    }


    public static IServiceCollection AddEventBus(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);

        var rabbitMQConfig = new List<RabbitMqSettings>();
        var rabbitMqConfigurations = configuration.GetSection("RabbitMqSettings").Get<List<RabbitMqSettings>>();

        var config = rabbitMqConfigurations.FirstOrDefault(y => y.Name == "MainHost");
        if (config == null) throw new ArgumentNullException("MainHost section hasn't been found in the appsettings.");

        services.AddMassTransit<IEventBus>(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var mediator = context.GetRequiredService<IMediator>();
                cfg.Host(config.RabbitMqHostUrl, config.VirtualHost, h =>
                {
                    h.Username(config.Username);
                    h.Password(config.Password);
                });

                cfg.UseJsonSerializer();
                cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));
                cfg.ConfigureEndpoints(context);

                //cfg.ReceiveEndpoint(queueConfiguration.Names[QueueState.CreateOrder], e =>
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

                //    e.Consumer(() => new StockReleasedConsumer(mediator));
                //});
            });
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

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());


        return services;

    }
}
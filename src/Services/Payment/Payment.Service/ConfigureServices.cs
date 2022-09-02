using Core.Domain;
using Core.Domain.Bus;
using Core.Domain.Enums;
using MassTransit;
using MediatR;
using Payment.Application.Consumer;
using Payment.Service.Services;

namespace Payment.Service;

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

        var rabbitMqConfigurations = configuration.GetSection("RabbitMqSettings").Get<List<RabbitMqSettings>>();

        var config = rabbitMqConfigurations.FirstOrDefault(y => y.Name == "MainHost");
        if (config == null) throw new ArgumentNullException("MainHost section hasn't been found in the appsettings.");


        services.AddMassTransit<IEventBus>(x =>
        {
            x.AddConsumer<CardPaymentConsumer>();
            x.AddConsumer<FreeDeliveryConsumer>();
            x.AddConsumer<PayAtDoorConsumer>();

            x.SetKebabCaseEndpointNameFormatter();

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

                cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CardPayment], e =>
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
                    e.ConfigureConsumer<CardPaymentConsumer>(context);
                });

                cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.FreeDelivery], e =>
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
                    e.ConfigureConsumer<FreeDeliveryConsumer>(context);
                });

                cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.PayAtDoor], e =>
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
                    e.ConfigureConsumer<PayAtDoorConsumer>(context);
                });
            });
        });


        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromSeconds(30);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

        var bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(config.RabbitMqHostUrl, config.VirtualHost, h =>
            {
                h.Username(config.Username);
                h.Password(config.Password);
            });
        });

        services.AddSingleton<IPublishEndpoint>(bus);
        services.AddSingleton<ISendEndpointProvider>(bus);
        services.AddSingleton<IBus>(bus);
        services.AddSingleton<IBusControl>(bus);

        return services;

    }
}
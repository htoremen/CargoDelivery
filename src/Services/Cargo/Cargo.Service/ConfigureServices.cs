using Core.Domain;
using Core.Domain.Bus;
using MassTransit;
using MediatR;
using Cargo.Service.Services;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using Cargos;

namespace Cargo.Service;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        return services;
    }


    public static IServiceCollection AddEventBus(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);
        var messageBroker = appSettings.MessageBroker;

        services.AddMassTransit<IEventBus>(x =>
        {
            x.AddConsumer<CreateCargoConsumer>();
            x.AddConsumer<SendSelfieConsumer>();
            x.AddConsumer<CargoApprovalConsumer>();
            x.AddConsumer<CargoRejectedConsumer>();

            x.AddRequestClient<ISendSelfie>(new Uri("rabbitmq://localhost/Cargo.SendSelfie"));
            x.SetKebabCaseEndpointNameFormatter();

            if(messageBroker.UsedRabbitMQ())
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
            cfg.UseRetry(c => c.Interval(config.RetryCount, config.ResetInterval));
            cfg.ConfigureEndpoints(context);

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CreateCargo], e =>
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
                e.ConfigureConsumer<CreateCargoConsumer>(context);
            });

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

        });
    }
}
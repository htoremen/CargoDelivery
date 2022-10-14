using Core.Domain;
using Core.Domain.Bus;
using MassTransit;
using MediatR;
using Cargo.Service.Services;
using Core.Domain.Enums;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using Cargo.GRPC.Server.Services;
using Core.Infrastructure.Common.Extensions;

namespace Cargo.Service;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        return services;
    }

    public static WebApplication MapGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<DebitService>();
        app.MapGrpcService<CargoService>();
        app.MapGrpcService<CargoHealthService>();
        return app;
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

    public static IServiceCollection AddEventBus(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);
        var messageBroker = appSettings.MessageBroker;

        services.AddMassTransit<IEventBus>(x => { UsingRabbitMq(x, messageBroker, queueConfiguration); });

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

        return services;
    }
    private static void UsingRabbitMq(IBusRegistrationConfigurator<IEventBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        x.SetKebabCaseEndpointNameFormatter();
        x.SetSnakeCaseEndpointNameFormatter();

        x.AddConsumer<CreateDebitConsumer, CreateDebitConsumerDefinition>();
        x.AddConsumer<SendSelfieConsumer, SendSelfieConsumerDefinition>();
        x.AddConsumer<CargoApprovalConsumer, CargoApprovalConsumerDefinition>();
        x.AddConsumer<CargoRejectedConsumer, CargoRejectedConsumerDefinition>();

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

            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CreateDebit], e => { e.ConfigureConsumer<CreateDebitConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.SendSelfie], e => { e.ConfigureConsumer<SendSelfieConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoApproval], e => { e.ConfigureConsumer<CargoApprovalConsumer>(context); });
            cfg.ReceiveEndpoint(queueConfiguration.Names[QueueName.CargoRejected], e => { e.ConfigureConsumer<CargoRejectedConsumer>(context); });

            cfg.ConfigureEndpoints(context);
        });
    }
}
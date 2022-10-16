using Core.Domain;
using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using Core.Infrastructure.MessageBrokers;
using Core.Infrastructure.Telemetry.Options;
using MassTransit;
using MediatR;
using Order.API.Services;
using Order.API.Telemetry;
using Order.Infrastructure.Healths;

namespace Order.API;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        return services;
    }

    public static IServiceCollection AddTelemetryTracingOrderServices(this IServiceCollection services, AppSettings appSettings)
    {
        var options = new OpenTelemetryOptions
        {
            RedisConfiguration = appSettings.Caching.Distributed.Redis.Configuration,
            AgentHost=appSettings.Telemetry.Jaeger.AgentHost,
            AgentPort=appSettings.Telemetry.Jaeger.AgentPort,
            ServiceName= OpenTelemetryExtensions.ServiceName,
            ServiceVersion = OpenTelemetryExtensions.ServiceVersion
        };

        services.AddOpenTelemetryTracingServices(options);

        return services;
    }

    //public static IServiceCollection OpenTelemetryTranckingServices(this IServiceCollection services)
    //{
    //    var SourceName = "Order.APi";
    //    var MeterName = "";
    //    services.AddOpenTelemetryTracing(options =>
    //            options
    //                .AddSource(SourceName)
    //                .SetResourceBuilder(resourceBuilder: ResourceBuilder.CreateDefault().AddService(SourceName).AddTelemetrySdk())
    //                .AddSqlClientInstrumentation(options =>
    //                {
    //                    options.SetDbStatementForText = true;
    //                    options.RecordException = true;
    //                })
    //                .AddAspNetCoreInstrumentation()
    //                .AddHttpClientInstrumentation()
    //                .SetErrorStatusOnException(true)
    //                .AddOtlpExporter(options =>
    //                {
    //                    options.Endpoint = new Uri("http://localhost:4317");
    //                })
    //                );

    //    services.AddOpenTelemetryMetrics(options =>
    //    {
    //        options.AddHttpClientInstrumentation()
    //               .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(SourceName).AddTelemetrySdk())
    //               .AddMeter(MeterName)
    //               .AddOtlpExporter(options =>
    //                {
    //                    options.Endpoint = new Uri("http://localhost:4317");
    //                });
    //    });

    //    services.Configure<AspNetCoreInstrumentationOptions>(options =>
    //    {
    //        options.RecordException = true;
    //    });

    // //   services.AddSingleton<ComputerVisionMetricsService>

    //    return services;
    //}

    //public static IServiceCollection OpenTracingServices(this IServiceCollection services)
    //{
    //    services.AddOpenTracing();
    //    // Adds the Jaeger Tracer.
    //    services.AddSingleton<ITracer>(sp =>
    //    {
    //        var serviceName = sp.GetRequiredService<IWebHostEnvironment>().ApplicationName;
    //        var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
    //        var reporter = new RemoteReporter.Builder().WithLoggerFactory(loggerFactory).WithSender(new UdpSender())
    //            .Build();
    //        var tracer = new Tracer.Builder(serviceName)
    //            // The constant sampler reports every span.
    //            .WithSampler(new ConstSampler(true))
    //            // LoggingReporter prints every reported span to the logging framework.
    //            .WithReporter(reporter)
    //            .Build();
    //        return tracer;
    //    });

    //    services.Configure<HttpHandlerDiagnosticOptions>(options =>
    //    options.OperationNameResolver =
    //        request => $"{request.Method.Method}: {request?.RequestUri?.AbsoluteUri}");
    //    return services;
    //}

    public static IServiceCollection AddHealthChecksServices(this IServiceCollection services, AppSettings appSettings)
    {
            services.AddHealthChecks()
                .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings))
                .AddCheck<RedisHealtCheck>("order-redis");
        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);
        var messageBroker = appSettings.MessageBroker;
        services.AddMassTransit(x => { UsingRabbitMq(x, messageBroker, queueConfiguration); });
        services.ConfigureMassTransitHostOptions(messageBroker);

        return services;
    }

    private static void UsingRabbitMq(IBusRegistrationConfigurator x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        x.SetKebabCaseEndpointNameFormatter();
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
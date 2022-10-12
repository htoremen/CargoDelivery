using Core.Domain;
using Core.Domain.Bus;
using Core.Infrastructure;
using Core.Infrastructure.Common.Extensions;
using Core.Infrastructure.MessageBrokers;
using MassTransit;
using MediatR;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Order.API.Services;
using Order.API.Telemetry;
using Order.Infrastructure.Healths;
using StackExchange.Redis;

namespace Order.API;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        return services;
    }

    /// <summary>
    /// https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOpenTelemetryTracingServices(this IServiceCollection services, AppSettings appSettings)
    {
        var multiplexer = ConnectionMultiplexer.Connect(appSettings.Caching.Distributed.Redis.Configuration);

        //Action<ResourceBuilder> configureResource = r => r.AddService(OpenTelemetryExtensions.ServiceName, OpenTelemetryExtensions.ServiceVersion);

        services.AddOpenTelemetryTracing(traceProvider =>
        {
            traceProvider
                .AddSource(OpenTelemetryExtensions.ServiceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(OpenTelemetryExtensions.ServiceName, OpenTelemetryExtensions.ServiceVersion))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                //.Configure((sp, builder) =>
                //{
                //    RedisCache cache = (RedisCache)sp.GetRequiredService<IDistributedCache>();
                //    builder.AddRedisInstrumentation(cache.GetConnection());
                //})
                .AddRedisInstrumentation(multiplexer, options => options.SetVerboseDatabaseStatements = true)
                .AddJaegerExporter(exporter =>
                {
                    exporter.AgentHost = appSettings.Telemetry.Jaeger.AgentHost;
                    exporter.AgentPort = Convert.ToInt32(appSettings.Telemetry.Jaeger.AgentPort);
                    exporter.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
                });
        });

        services.Configure<OpenTelemetryLoggerOptions>(opt =>
        {
            opt.IncludeScopes = true;
            opt.ParseStateValues = true;
            opt.IncludeFormattedMessage = true;
        });


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
        var messageBroker = appSettings.MessageBroker;
        if (messageBroker.UsedRabbitMQ())
        {
            services.AddHealthChecks()
                .AddRabbitMQ(GeneralExtensions.GetRabbitMqConnection(appSettings));
        }

        services.AddHealthChecks()
            .AddCheck<RedisHealtCheck>("order-redis");

        return services;
    }

    public static IServiceCollection AddEventBus(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddQueueConfiguration(out IQueueConfiguration queueConfiguration);
        var messageBroker = appSettings.MessageBroker;
        services.AddMassTransit<IEventBus>(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            if (messageBroker.UsedRabbitMQ())
                UsingRabbitMq(x, messageBroker, queueConfiguration);
            else if (messageBroker.UsedKafka())
                UsingKafka(x, messageBroker, queueConfiguration);
            
        });

        if (messageBroker.UsedRabbitMQ())
        {
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
        }
        else if (messageBroker.UsedKafka())
        {

            //using var provider = services.BuildServiceProvider(true);
            //var busControl = provider.GetRequiredService<IBusControl>();
            //var startTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            //busControl.StartAsync(startTokenSource);
        }
        return services;
    }

    private static void UsingKafka(IBusRegistrationConfigurator<IEventBus> x, MessageBrokerOptions messageBroker, IQueueConfiguration queueConfiguration)
    {
        var config = messageBroker.Kafka;
        x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context, SnakeCaseEndpointNameFormatter.Instance));
        x.AddRider(rider =>
        {
            rider.UsingKafka((context, k) =>
            {
                var mediator = context.GetRequiredService<IMediator>();
                k.Host(config.BootstrapServers);
            });
        }); 
        
        x.AddOptions<MassTransitHostOptions>().Configure(options =>
        {
            options.WaitUntilStarted = true;
        });
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
            cfg.UseMessageRetry(r => r.Immediate(5));

            cfg.ConfigureEndpoints(context);
        });
    }
}
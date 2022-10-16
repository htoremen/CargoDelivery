using MassTransit;
using Core.Infrastructure.Telemetry.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;
using Core.Infrastructure.Telemetry;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection ConfigureMassTransitHostOptions(this IServiceCollection services, MessageBrokerOptions messageBroker)
    {
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.Configure<MassTransitHostOptions>(options =>
        {
            options.WaitUntilStarted = true;
            options.StartTimeout = TimeSpan.FromMinutes(5);
            options.StopTimeout = TimeSpan.FromMinutes(1);
        });

        var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
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

    /// <summary>
    /// https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/
    /// </summary>
    /// <param name="services"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    public static IServiceCollection AddOpenTelemetryTracingServices(this IServiceCollection services, OpenTelemetryOptions options)
    {
        ConfigurationOptions option = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            EndPoints = { options.RedisConfiguration }
        };
        var multiplexer = ConnectionMultiplexer.Connect(option);

        //Action<ResourceBuilder> configureResource = r => r.AddService(OpenTelemetryExtensions.ServiceName, OpenTelemetryExtensions.ServiceVersion);

        services.AddOpenTelemetryTracing(traceProvider =>
        {
            traceProvider
                .AddSource(options.ServiceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(options.ServiceName, null, options.ServiceVersion))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                //.Configure((sp, builder) =>
                //{
                //    RedisCache cache = (RedisCache)sp.GetRequiredService<IDistributedCache>();
                //    builder.AddRedisInstrumentation(cache.GetConnection());
                //})
                .AddRedisInstrumentation(multiplexer, options => options.SetVerboseDatabaseStatements = true)
                //.AddMassTransitLegacySource()
                .AddJaegerExporter(exporter =>
                {
                    exporter.AgentHost = options.AgentHost;
                    exporter.AgentPort = Convert.ToInt32(options.AgentPort);
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
}

using System.Reflection;
using Cargo.Application.Common.Behaviours;
using Cargo.Application.Telemetry;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using Core.Infrastructure.Telemetry.Options;
using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using NoSQLMongo.Infrastructure;
using Shipments;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppSettings appSettings, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));

        services
            .AddMessageBusSender<ICreateDebit>(appSettings.MessageBroker)
           // .AddMessageBusSender<ICreateDebitFault>(appSettings.MessageBroker)
           // .AddMessageBusSender<ICreateCargo>(appSettings.MessageBroker)
            .AddMessageBusSender<ISendSelfie>(appSettings.MessageBroker)
            .AddMessageBusSender<ICargoApproval>(appSettings.MessageBroker)
            .AddMessageBusSender<ICargoRejected>(appSettings.MessageBroker)
            .AddMessageBusSender<IShipmentReceived>(appSettings.MessageBroker)
           //.AddMessageBusSender<ICreateDebitHistory>(appSettings.MessageBroker)
            ;

        services.AddNoSQLMongoServices(appSettings.MongoDbSettings);

        return services;
    }

    public static IServiceCollection AddTelemetryTracingServices(this IServiceCollection services, AppSettings appSettings)
    {
        var options = new OpenTelemetryOptions
        {
            RedisConfiguration = appSettings.Caching.Distributed.Redis.Configuration,
            AgentHost = appSettings.Telemetry.Jaeger.AgentHost,
            AgentPort = appSettings.Telemetry.Jaeger.AgentPort,
            ServiceName = OpenTelemetryExtensions.ServiceName,
            ServiceVersion = OpenTelemetryExtensions.ServiceVersion
        };

        services.AddOpenTelemetryTracingServices(options);

        return services;
    }
}

using System.Reflection;
using Cargo.Application.Common.Behaviours;
using Core.Domain.MessageBrokers;
using Core.Infrastructure.MessageBrokers.RabbitMQ;
using FluentValidation;
namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services.AddMessageBusSender<IStartRoute>();

        return services;
    }

    public static IServiceCollection AddRabbitMQSender<T>(this IServiceCollection services)
    {
        services.AddSingleton<IMessageSender<T>, RabbitMQSender<T>>();
        return services;
    }

    public static IServiceCollection AddMessageBusSender<T>(this IServiceCollection services, IHealthChecksBuilder healthChecksBuilder = null, HashSet<string> checkDulicated = null)
    {
        services.AddRabbitMQSender<T>();
        return services;
    }
}

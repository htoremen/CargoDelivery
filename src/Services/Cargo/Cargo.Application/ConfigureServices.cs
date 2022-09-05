using System.Reflection;
using Cargo.Application.Common.Behaviours;
using Core.Infrastructure.MessageBrokers;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services
            .AddMessageBusSender<ICreateCargo>(null)
            .AddMessageBusSender<ISendSelfie>(null)
            .AddMessageBusSender<ICargoApproval>(null)
            .AddMessageBusSender<ICargoRejected>(null)
            .AddMessageBusSender<IStartRoute>(null);

        return services;
    }
}

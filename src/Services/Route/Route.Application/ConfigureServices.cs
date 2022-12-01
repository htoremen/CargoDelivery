using System.Reflection;
using Route.Application.Common.Behaviours;
using FluentValidation;
using Core.Infrastructure;

using Core.Infrastructure.MessageBrokers;
using Core.Infrastructure.Cache;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services.AddCaches(appSettings.Caching);

        services
           .AddMessageBusSender<IStartDelivery>(appSettings.MessageBroker)
           .AddMessageBusSender<IDebitApproval>(appSettings.MessageBroker);

        services.AddGrpcServices();
        return services;
    }
}

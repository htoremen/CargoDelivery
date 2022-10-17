using Cargos;
using Core.Application.Common.Behaviours;
using Core.Infrastructure;
using Core.Infrastructure.Cache;
using Core.Infrastructure.MessageBrokers;
using Deliveries;
using FluentValidation;
using MediatR.Pipeline;
using Shipment.Application.Common.Behaviours;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddCaches(appSettings.Caching);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddTransient(typeof(IRequestPreProcessor<>), typeof(LoggingBehaviour<>));

        services
            .AddMessageBusSender<ISendMail>(appSettings.MessageBroker)
            .AddMessageBusSender<ISendSms>(appSettings.MessageBroker)
            .AddMessageBusSender<IPushNotification>(appSettings.MessageBroker)
            .AddMessageBusSender<IStartRoute>(appSettings.MessageBroker)
            .AddMessageBusSender<IDeliveryCompleted>(appSettings.MessageBroker);

        services.AddGrpcServices();
        return services;
    }
}

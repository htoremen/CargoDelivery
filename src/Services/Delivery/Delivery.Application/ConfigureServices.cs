using System.Reflection;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using Delivery.Application.Common.Behaviours;
using FluentValidation;
using Notifications;
using Payments;

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

        services
            .AddMessageBusSender<IStartDelivery>(appSettings.MessageBroker)
            .AddMessageBusSender<INewDelivery>(appSettings.MessageBroker)

            .AddMessageBusSender<ISendMail>(appSettings.MessageBroker)
            .AddMessageBusSender<ISendSms>(appSettings.MessageBroker)
            .AddMessageBusSender<IPushNotification>(appSettings.MessageBroker)
            .AddMessageBusSender<IVerificationCode>(appSettings.MessageBroker)

            .AddMessageBusSender<ICardPayment>(appSettings.MessageBroker)
            .AddMessageBusSender<IPayAtDoor>(appSettings.MessageBroker)
            .AddMessageBusSender<IFreeDelivery>(appSettings.MessageBroker)
            .AddMessageBusSender<IDeliveryCompleted>(appSettings.MessageBroker)
            .AddMessageBusSender<IShiftCompletion>(appSettings.MessageBroker);

        services.AddGrpcServices();
        return services;
    }
}
﻿using System.Reflection;
using Order.Application.Common.Behaviours;
using FluentValidation;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using Core.Infrastructure.Cache;


namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services.AddCaches(appSettings.Caching);

        services
            .AddMessageBusSender<ICreateDebit>(appSettings.MessageBroker)
            .AddMessageBusSender<ISendSelfie>(appSettings.MessageBroker)
            .AddMessageBusSender<IDebitApproval>(appSettings.MessageBroker)
            .AddMessageBusSender<IDebitRejected>(appSettings.MessageBroker)

            .AddMessageBusSender<IStartRoute>(appSettings.MessageBroker)
            .AddMessageBusSender<IManuelRoute>(appSettings.MessageBroker)
            .AddMessageBusSender<IAutoRoute>(appSettings.MessageBroker)

            .AddMessageBusSender<IStartDelivery>(appSettings.MessageBroker)
            .AddMessageBusSender<IStartDistribution>(appSettings.MessageBroker)
            .AddMessageBusSender<IVerificationCode>(appSettings.MessageBroker)

            .AddMessageBusSender<ICreateDelivery>(appSettings.MessageBroker)
            .AddMessageBusSender<ICreateRefund>(appSettings.MessageBroker)
            .AddMessageBusSender<INotDelivered>(appSettings.MessageBroker)

            .AddMessageBusSender<IDeliveryCompleted>(appSettings.MessageBroker)
            .AddMessageBusSender<IShiftCompletion>(appSettings.MessageBroker);
        return services;
    }
}

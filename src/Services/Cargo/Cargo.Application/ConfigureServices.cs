﻿using System.Reflection;
using Cargo.Application.Common.Behaviours;
using Core.Infrastructure;
using Core.Infrastructure.MessageBrokers;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using NoSQLMongo.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppSettings appSettings, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services
            .AddMessageBusSender<ICreateCargo>(appSettings.MessageBroker)
            .AddMessageBusSender<ISendSelfie>(appSettings.MessageBroker)
            .AddMessageBusSender<ICargoApproval>(appSettings.MessageBroker)
            .AddMessageBusSender<ICargoRejected>(appSettings.MessageBroker)
            .AddMessageBusSender<IStartRoute>(appSettings.MessageBroker);

        services.AddNoSQLMongoServices(appSettings.MongoDbSettings);

        return services;
    }
}

﻿using Core.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Route.GRPC.Client.Services;
using Route.Infrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext(appSettings);
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }

    private static void AddDbContext(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                appSettings.ConnectionStrings.ConnectionString,
                configure =>
                {
                    configure.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    configure.EnableRetryOnFailure();
                }), ServiceLifetime.Scoped);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }
}

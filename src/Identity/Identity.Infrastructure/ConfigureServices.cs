using Core.Infrastructure;
using Identity.Application.Common.Interfaces;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }

    private static void AddDbContext(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                appSettings.ConnectionStrings.CargoConnectionString,
                configure =>
                {
                    configure.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    configure.EnableRetryOnFailure();
                }), ServiceLifetime.Scoped);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }
}

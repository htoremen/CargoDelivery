
using HangfireJob.Infrastructure.Identity;
using HangfireJob.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}

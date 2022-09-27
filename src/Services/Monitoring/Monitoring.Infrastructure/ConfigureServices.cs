

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddGrpc();
        services.AddHealthChecks()
            .AddCheck<CargoHealthCheck>("Cargo");

        return services;
    }

}

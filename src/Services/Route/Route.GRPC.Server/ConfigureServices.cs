using Core.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services, AppSettings appSettings, Configuration.IConfiguration configuration)
    {
        services.AddGrpc();

        return services;
    }
}

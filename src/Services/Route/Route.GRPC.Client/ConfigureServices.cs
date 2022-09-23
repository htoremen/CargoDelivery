using Route.GRPC.Client.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddSingleton<IDebitService, DebitService>();
        services.AddSingleton<ICargoService, CargoService>();
        return services;
    }
}

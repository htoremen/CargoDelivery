using Delivery.GRPC.Client;
using Delivery.GRPC.Client.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddSingleton<IRouteService, RouteService>();
        services.AddSingleton<ICargoService, CargoService>();
        return services;
    }
}

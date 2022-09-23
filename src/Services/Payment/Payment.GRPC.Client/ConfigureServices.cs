using Payment.GRPC.Client.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddSingleton<IDeliveryService, DeliveryService>();
        return services;
    }
}

using Core.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddApplicationCommon();

     //   services
      //      .AddMessageBusSender<IDeliveryCompleted>(appSettings.MessageBroker);

        return services;
    }
}

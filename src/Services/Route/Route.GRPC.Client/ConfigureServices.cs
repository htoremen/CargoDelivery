using Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services, AppSettings appSettings, Configuration.IConfiguration configuration)
    {

        return services;
    }
}

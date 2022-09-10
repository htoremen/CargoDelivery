using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NoSQLMongo.Application.Common.Interfaces;
using NoSQLMongo.Infrastructure.Repositories;
using NoSQLMongo.Infrastructure.Settings;

namespace NoSQLMongo.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddNoSQLMongoServices(this IServiceCollection services, MongoDbSettings mongoDbSettings)
    {
        services.AddSingleton<IMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

        services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

        return services;
    }
}

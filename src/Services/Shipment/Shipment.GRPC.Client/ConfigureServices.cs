﻿using Shipment.GRPC.Client.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddGrpcServices(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddSingleton<IDebitService, DebitService>();
        return services;
    }
}

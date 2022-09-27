using Grpc.Net.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Route.GRPC.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Route.GRPC.Server.RouteHealthCheckResponse.Types;
using static Route.GRPC.Server.RouteHealthGrpc;

namespace Delivery.Infrastructure.Healths;

public class RouteHealthCheck : IHealthCheck
{
    private readonly ILogger<RouteHealthCheck> _logger;
    private GrpcChannel Channel { get; set; }
    private RouteHealthGrpcClient Client { get; set; }

    public RouteHealthCheck(ILogger<RouteHealthCheck> logger)
    {
        _logger = logger;
        Channel = GrpcChannel.ForAddress("https://localhost:5012");
        Client = new RouteHealthGrpcClient(Channel);
    }
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Starting RouteCheckHealthAsync");
            var result = await Client.CheckAsync(new RouteHealthCheckRequest { Service = "Route.GRPC.Server" });

            if (result.Status == RouteStatus.NotServing)
                return HealthCheckResult.Unhealthy();

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Starting RouteCheckHealthAsync");
            return HealthCheckResult.Unhealthy();
        }
    }
}

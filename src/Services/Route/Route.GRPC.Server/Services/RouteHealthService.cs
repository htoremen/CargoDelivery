using Grpc.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Route.GRPC.Server.RouteHealthCheckResponse.Types;

namespace Route.GRPC.Server.Services;

public class RouteHealthService : RouteHealthGrpc.RouteHealthGrpcBase
{
    private readonly HealthCheckService healthCheckService;
    private readonly ILogger<RouteHealthService> logger;

    public RouteHealthService(HealthCheckService healthCheckService, ILogger<RouteHealthService> logger)
    {
        this.healthCheckService = healthCheckService;
        this.logger = logger;
    }

    public async override Task<RouteHealthCheckResponse> Check(RouteHealthCheckRequest request, ServerCallContext context)
    {
        logger.LogDebug("Starting HealthCheck");
        var result = await healthCheckService.CheckHealthAsync(context.CancellationToken);
        var status = result.Status == HealthStatus.Healthy ? RouteStatus.Serving : RouteStatus.NotServing;
        return new RouteHealthCheckResponse
        {
            Status = status
        };
    }
}

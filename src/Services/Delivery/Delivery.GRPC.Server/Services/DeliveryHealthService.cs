using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Delivery.GRPC.Server.DeliveryHealthCheckResponse.Types;

namespace Delivery.GRPC.Server.Services;

public class DeliveryHealthService : DeliveryHealthGrpc.DeliveryHealthGrpcBase
{
    private readonly HealthCheckService healthCheckService;
    private readonly ILogger<DeliveryHealthService> logger;

    public DeliveryHealthService(HealthCheckService healthCheckService, ILogger<DeliveryHealthService> logger)
    {
        this.healthCheckService = healthCheckService;
        this.logger = logger;
    }

    public async override Task<DeliveryHealthCheckResponse> Check(DeliveryHealthCheckRequest request, ServerCallContext context)
    {
        logger.LogDebug("Starting HealthCheck");
        var result = await healthCheckService.CheckHealthAsync(context.CancellationToken);
        var status = result.Status == HealthStatus.Healthy ? DeliveryStatus.Serving : DeliveryStatus.NotServing;
        return new DeliveryHealthCheckResponse
        {
            Status = status
        };
    }
}
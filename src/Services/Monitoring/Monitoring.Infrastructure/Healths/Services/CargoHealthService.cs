using Cargo.GRPC.Server;
using Grpc.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cargo.GRPC.Server.CargoHealthCheckResponse.Types;

namespace Monitoring.Infrastructure.Healths.Services;

public class CargoHealthService : CargoHealthGrpc.CargoHealthGrpcBase
{
    private readonly HealthCheckService healthCheckService;
    private readonly ILogger<CargoHealthService> logger;

    public CargoHealthService(HealthCheckService healthCheckService, ILogger<CargoHealthService> logger)
    {
        this.healthCheckService = healthCheckService;
        this.logger = logger;
    }

    public async override Task<CargoHealthCheckResponse> Check(CargoHealthCheckRequest request, ServerCallContext context)
    {
        logger.LogDebug("Starting HealthCheck");
        var result = await healthCheckService.CheckHealthAsync(context.CancellationToken);
        var status = result.Status == HealthStatus.Healthy ? ServingStatus.Serving : ServingStatus.NotServing;
        return new CargoHealthCheckResponse
        {
            Status = status
        };
    }
}
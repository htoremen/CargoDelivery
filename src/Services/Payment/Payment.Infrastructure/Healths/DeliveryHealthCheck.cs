using Delivery.GRPC.Server;
using Grpc.Net.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Delivery.GRPC.Server.DeliveryHealthCheckResponse.Types;
using static Delivery.GRPC.Server.DeliveryHealthGrpc;

namespace Payment.Infrastructure.Healths;

public class DeliveryHealthCheck : IHealthCheck
{
    private readonly ILogger<DeliveryHealthCheck> _logger;
    private GrpcChannel Channel { get; set; }
    private DeliveryHealthGrpcClient Client { get; set; }

    public DeliveryHealthCheck(ILogger<DeliveryHealthCheck> logger)
    {
        _logger = logger;
        Channel = GrpcChannel.ForAddress("https://localhost:5013");
        Client = new DeliveryHealthGrpcClient(Channel);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Starting CheckHealthAsync");
            var result = await Client.CheckAsync(new DeliveryHealthCheckRequest { Service = "Delivery.GRPC.Server" });

            if (result.Status == DeliveryStatus.NotServing)
                return HealthCheckResult.Unhealthy();

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Starting CheckHealthAsync");
            return HealthCheckResult.Unhealthy();
        }
    }
}

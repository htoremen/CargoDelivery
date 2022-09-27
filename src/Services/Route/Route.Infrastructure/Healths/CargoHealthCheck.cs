using Cargo.GRPC.Server;
using Grpc.Net.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using static Cargo.GRPC.Server.CargoHealthCheckResponse.Types;
using static Cargo.GRPC.Server.CargoHealthGrpc;

namespace Route.Infrastructure.Healths;

public class CargoHealthCheck : IHealthCheck
{
    private readonly ILogger<CargoHealthCheck> _logger;
    private GrpcChannel Channel { get; set; }
    private CargoHealthGrpcClient Client { get; set; }

    public CargoHealthCheck(ILogger<CargoHealthCheck> logger)
    {
        _logger = logger;
        Channel = GrpcChannel.ForAddress("https://localhost:5011");
        Client = new CargoHealthGrpcClient(Channel);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Starting CheckHealthAsync");
            var result = await Client.CheckAsync(new CargoHealthCheckRequest { Service = "Cargo.GRPC.Server" });

            if (result.Status == ServingStatus.NotServing)
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
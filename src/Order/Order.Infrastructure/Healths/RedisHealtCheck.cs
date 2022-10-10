using Core.Application.Common.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Order.Infrastructure.Healths;

public class RedisHealtCheck : IHealthCheck
{
    private readonly ILogger<RedisHealtCheck> _logger;
    private readonly ICacheService _cacheService;

    public RedisHealtCheck(ILogger<RedisHealtCheck> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Starting RedisHealtCheck");
            await _cacheService.SetAsync("healtcheck", 1);

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, "Starting RedisHealtCheck");
            return HealthCheckResult.Unhealthy();
        }
    }
}


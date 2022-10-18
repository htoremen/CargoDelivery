using Enums;
using Microsoft.AspNetCore.Mvc;
using Order.Application.NoSqls.RedisDataAdds;
using Order.Application.Shipments.StartDistributions;
using Core.Domain;
using System.Text.Json;

namespace Order.API.Controllers;

public class ShipmentController : ApiControllerBase
{
    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("start-distribution")]
    public async Task<IActionResult> StartDistribution(Guid correlationId, Guid cargoId, NotificationType notificationType)
    {
        var command = new StartDistributionCommand
        {
            CargoId = cargoId,
            CorrelationId = correlationId,
            NotificationType = notificationType
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.StartDistribution,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }
}
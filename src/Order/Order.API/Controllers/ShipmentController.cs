using Microsoft.AspNetCore.Mvc;
using Order.Application.NoSqls.RedisDataAdds;
using Order.Application.Shipments.StartDistributions;
using Order.Domain;
using System.Text.Json;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShipmentController : ApiControllerBase
{
    public class DeliveryController : ApiControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Route("start-distribution")]
        public async Task<IActionResult> StartDistribution(Guid correlationId, Guid cargoId)
        {
            var command = new StartDistributionCommand
            {
                CargoId = cargoId,
                CorrelationId = correlationId
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
}
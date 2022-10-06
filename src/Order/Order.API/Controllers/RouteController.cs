using Microsoft.AspNetCore.Mvc;
using Order.Application.NoSqls.RedisDataAdds;
using Order.Application.Routes.AutoRoutes;
using Order.Application.Routes.ManuelRoutes;
using Order.Domain;
using System.Text.Json;

namespace Order.API.Controllers;

public class RouteController : ApiControllerBase
{
    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("auto-route")]

    public async Task<IActionResult> AutoRoute([FromBody] Guid correlationId)
    {
        var command = new AutoRouteCommand
        {
            CorrelationId = correlationId
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.AutoRoute,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("manuel-route")]
    public async Task<IActionResult> ManuelRoute([FromBody] Guid correlationId)
    {
        var command = new ManuelRouteCommand
        {
            CorrelationId = correlationId
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.ManuelRoute,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }
}

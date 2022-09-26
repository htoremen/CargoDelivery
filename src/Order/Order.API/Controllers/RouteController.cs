using Microsoft.AspNetCore.Mvc;
using Order.Application.Routes.AutoRoutes;
using Order.Application.Routes.ManuelRoutes;

namespace Order.API.Controllers;

public class RouteController : ApiControllerBase
{
    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("auto-route")]

    public async Task<IActionResult> AutoRoute([FromBody] Guid correlationId)
    {
        var response = await Mediator.Send(new AutoRouteCommand
        {
            CorrelationId = correlationId

        });
        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("manuel-route")]
    public async Task<IActionResult> ManuelRoute([FromBody] Guid correlationId)
    {
        var response = await Mediator.Send(new ManuelRouteCommand
        {
            CorrelationId = correlationId

        });
        return Ok(response);
    }
}

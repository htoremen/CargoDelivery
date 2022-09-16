using Microsoft.AspNetCore.Mvc;
using Order.Application.Routes.AutoRoutes;
using Order.Application.Routes.ManuelRoutes;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RouteController : ApiControllerBase
{ 
    [Route("auto-route")]
    [HttpPost]
    public async Task<IActionResult> AutoRoute(Guid correlationId)
    {
        var response = await Mediator.Send(new AutoRouteCommand
        {
            CargoId = Guid.NewGuid(),
            CorrelationId = correlationId

        });
        return Ok(response);
    }

    [Route("manuel-route")]
    [HttpPost]
    public async Task<IActionResult> ManuelRoute(Guid correlationId)
    {
        var response = await Mediator.Send(new ManuelRouteCommand
        {
            CargoId = Guid.NewGuid(),
            CorrelationId = correlationId

        });
        return Ok(response);
    }
}

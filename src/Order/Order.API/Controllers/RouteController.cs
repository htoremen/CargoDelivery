using Microsoft.AspNetCore.Mvc;
using Order.Application.Routes.RouteConfirmeds;

namespace Order.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RouteController : ApiControllerBase
{
    [Route("route-confirmed")]
    [HttpPost]
    public async Task<IActionResult> RouteConfirmed()
    {
        var response = await Mediator.Send(new RouteConfirmedCommand
        {
            CargoId = Guid.NewGuid()
        });
        return Ok(response);
    }
}

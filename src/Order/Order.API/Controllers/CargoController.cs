using Cargo.Application.Orders.CreateOrders;
using Cargo.Application.Orders.OrderApproveds;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Orders.CreateSelfies;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ApiControllerBase
    {
        [Route("create-cargo")]
        [HttpPost]
        public async Task<IActionResult> CreateCargo()
        {
            var response = await Mediator.Send(new CreateCargoCommand
            {
                CargoId = Guid.NewGuid(),   
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                ProductId= Guid.NewGuid(),
            });
            return Ok(response);
        }

        [Route("create-selfie")]
        [HttpPost]
        public async Task<IActionResult> CreateSelfie(Guid correlationId)
        {
            var response = await Mediator.Send(new CreateSelfieCommand
            {
                CargoId = Guid.NewGuid(),
                CorrelationId = correlationId
            });
            return Ok(response);
        }

        [Route("cargo-approved")]
        [HttpPost]
        public async Task<IActionResult> CargoApproved(Guid correlationId)
        {
            var response = await Mediator.Send(new CargoApprovedCommand
            {
                CargoId = Guid.NewGuid(),
                CorrelationId = correlationId
            });
            return Ok(response);
        }
    }
}

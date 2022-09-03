using Cargo.Application.Cargos.CreateOrders;
using Cargo.Application.Cargos.OrderApproveds;
using Cargo.Application.Cargos.SendSelfies;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Cargos.SendSelfieAgains;

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

        [Route("send-selfie")]
        [HttpPost]
        public async Task<IActionResult> SendSelfie(Guid correlationId)
        {
            var response = await Mediator.Send(new SendSelfieCommand
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

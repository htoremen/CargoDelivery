using Cargo.Application.Cargos.CargoApprovals;
using Cargo.Application.Cargos.CreateOrders;
using Cargo.Application.Cargos.SendSelfies;
using Microsoft.AspNetCore.Mvc;

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
                CourierId = Guid.NewGuid(),   
                DebitId = Guid.NewGuid()
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

        [Route("cargo-approval")]
        [HttpPost]
        public async Task<IActionResult> CargoApproval(Guid correlationId)
        {
            var response = await Mediator.Send(new CargoApprovalCommand
            {
                CargoId = Guid.NewGuid(),
                CorrelationId = correlationId
            });
            return Ok(response);
        }
    }
}

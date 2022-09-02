using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Deliveries.CreateDeliveries;
using Order.Application.Deliveries.CreateRefunds;
using Order.Application.Deliveries.NotDeliveries;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ApiControllerBase
    {
        [Route("create-delivery")]
        [HttpPost]
        public async Task<IActionResult> CreateDelivery(Guid correlationId, Guid cargoId)
        {
            var response = await Mediator.Send(new CreateDeliveryCommand
            {
                CargoId = cargoId,
                CorrelationId = correlationId

            });
            return Ok(response);
        }

        [Route("not-delivery")]
        [HttpPost]
        public async Task<IActionResult> NotDelivery(Guid correlationId, Guid cargoId)
        {
            var response = await Mediator.Send(new NotDeliveryCommand
            {
                CargoId = cargoId,
                CorrelationId = correlationId

            });
            return Ok(response);
        }

        [Route("create-refund")]
        [HttpPost]
        public async Task<IActionResult> CreateRefund(Guid correlationId, Guid cargoId)
        {
            var response = await Mediator.Send(new CreateRefundCommand
            {
                CargoId = cargoId,
                CorrelationId = correlationId

            });
            return Ok(response);
        }
    }
}

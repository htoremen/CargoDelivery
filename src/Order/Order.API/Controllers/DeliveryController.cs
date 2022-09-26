using Delivery.Application.Deliveries.ShiftCompletions;
using Microsoft.AspNetCore.Mvc;
using Delivery.Application.Deliveries.CreateDeliveries;
using Delivery.Application.Deliveries.CreateRefunds;
using Delivery.Application.Deliveries.NotDelivereds;
using Enums;

namespace Order.API.Controllers
{
    public class DeliveryController : ApiControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Route("create-delivery")]
        public async Task<IActionResult> CreateDelivery(Guid correlationId, Guid cargoId, PaymentType paymentType)
        {
            var response = await Mediator.Send(new CreateDeliveryCommand
            {
                CargoId = cargoId,
                CorrelationId = correlationId,
                PaymentType = paymentType
            });
            return Ok(response);
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Route("not-delivered")]
        public async Task<IActionResult> NotDelivered(Guid correlationId, Guid cargoId)
        {
            var response = await Mediator.Send(new NotDeliveredCommand
            {
                CargoId = cargoId,
                CorrelationId = correlationId

            });
            return Ok(response);
        }


        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Route("create-refund")]
        public async Task<IActionResult> CreateRefund(Guid correlationId, Guid cargoId)
        {
            var response = await Mediator.Send(new CreateRefundCommand
            {
                CargoId = cargoId,
                CorrelationId = correlationId

            });
            return Ok(response);
        }


        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Route("shift-completion")]
        public async Task<IActionResult> ShiftCompletion([FromBody] Guid correlationId)
        {
            var response = await Mediator.Send(new ShiftCompletionCommand
            {
                CorrelationId = correlationId

            });
            return Ok(response);
        }
    }
}

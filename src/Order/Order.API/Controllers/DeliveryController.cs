using Delivery.Application.Deliveries.ShiftCompletions;
using Microsoft.AspNetCore.Mvc;
using Delivery.Application.Deliveries.CreateDeliveries;
using Delivery.Application.Deliveries.CreateRefunds;
using Delivery.Application.Deliveries.NotDelivereds;
using Enums;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ApiControllerBase
    {
        [Route("create-delivery")]
        [HttpPost]
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

        [Route("not-delivered")]
        [HttpPost]
        public async Task<IActionResult> NotDelivered(Guid correlationId, Guid cargoId)
        {
            var response = await Mediator.Send(new NotDeliveredCommand
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

        [Route("shift-completion")]
        [HttpPost]
        public async Task<IActionResult> ShiftCompletion(Guid correlationId)
        {
            var response = await Mediator.Send(new ShiftCompletionCommand
            {
                CorrelationId = correlationId

            });
            return Ok(response);
        }
    }
}

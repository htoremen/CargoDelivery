using Microsoft.AspNetCore.Mvc;
using Order.Application.Orders.CreateOrders;
using Order.Application.Orders.CreateSelfies;
using Order.Application.Orders.OrderApproveds;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ApiControllerBase
    {
        [Route("create-order")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var response = await Mediator.Send(new CreateOrderCommand
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

        [Route("order-approved")]
        [HttpPost]
        public async Task<IActionResult> OrderApproved(Guid correlationId)
        {
            var response = await Mediator.Send(new OrderApprovedCommand
            {
                CargoId = Guid.NewGuid(),
                CorrelationId = correlationId
            });
            return Ok(response);
        }
    }
}

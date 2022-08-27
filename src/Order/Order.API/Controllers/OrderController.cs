using Microsoft.AspNetCore.Mvc;
using Order.Application.Orders.CreateOrders;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ApiControllerBase
    {
        [Route("Get")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await Mediator.Send(new CreateOrderCommand
            {
                OrderId = Guid.NewGuid(),   
                Id = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
                ProductId= Guid.NewGuid(),
            });
            return Ok(response);
        }
    }
}

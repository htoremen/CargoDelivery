using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ApiControllerBase
    {
        [Route("Get")]
        [HttpGet]
        public async Task<string> Get()
        {
            return await Mediator.Send(new CreateOrderCommand());
        }
    }
}

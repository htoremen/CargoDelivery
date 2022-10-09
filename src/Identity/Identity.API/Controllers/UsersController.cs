using Core.Domain.Models;
using Identity.Application.Authenticates.Commands.Logins;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<GenericResponse<LoginResponse>> Login(LoginCommand command)
        {
            var response = await Mediator.Send(command);
            return response;
        }
    }
}

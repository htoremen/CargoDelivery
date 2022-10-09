using Core.Domain.Models;
using Identity.Application.Authenticates.Commands.CreateUsers;
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

        [HttpPost]
        [Route("create-user")]
        public async Task<GenericResponse<CreateUserResponse>> CreateUser(CreateUserCommand command)
        {
            var response = await Mediator.Send(command);
            return response;
        }
    }
}

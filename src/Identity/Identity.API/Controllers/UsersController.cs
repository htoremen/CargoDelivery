using Core.Domain.Models;
using Identity.Application.Authenticates.Commands.CreateUsers;
using Identity.Application.Authenticates.Commands.Logins;
using Identity.Application.Authenticates.Commands.UserRefreshTokens;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
            command.IpAddress = ipAddress();
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


        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await Mediator.Send(new UserRefreshTokenCommand
            {
                IpAddress = ipAddress(),
                Token = refreshToken
            });
            setTokenCookie(response.Data.RefreshToken);
            return Ok(response);
        }

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
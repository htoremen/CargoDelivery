﻿using Identity.Domain.Entities;
using System.Text.Json.Serialization;

namespace Identity.Application.Authenticates.Commands.Logins;

public class LoginResponse
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }

    public LoginResponse(User user, string jwtToken, string refreshToken)
    {
        UserId = user.UserId;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Username = user.Username;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
    }
}

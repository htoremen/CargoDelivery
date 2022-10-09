using System.Text.Json.Serialization;

namespace Identity.Domain.Entities;

public class User
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }

    [JsonIgnore]
    public string PasswordHash { get; set; }

    [JsonIgnore]
    public List<RefreshToken> RefreshTokens { get; set; }
}
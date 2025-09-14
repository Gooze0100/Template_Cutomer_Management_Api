using System.IdentityModel.Tokens.Jwt;

namespace AuthApi.Settings;

public class AppSettings
{
    public required JwtPayload JwtPayload { get; set; } = new();
    public required JwtHeader JwtHeader { get; set; } = new();
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthApi.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthApi.Service;

public class TokenGenerator
{
    private readonly IOptions<AppSettings> _settings;
    public TokenGenerator(IOptions<AppSettings> settings)
    {
        _settings = settings;
    }
    public string GenerateToken(string email)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        var key = System.Text.Encoding.UTF8.GetBytes(_settings.Value.JwtHeader.Kid);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, email),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Iss, _settings.Value.JwtPayload.Iss),
            new(JwtRegisteredClaimNames.Aud, _settings.Value.JwtPayload.Aud[0])
        ];

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
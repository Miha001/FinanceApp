using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using Finances.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Finances.DAL.Implementations.Users;

public class TokenService(IOptions<JwtSettings> jwtSettings) : ITokenService
{
    /// <inheritdoc/>
    public string Create(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.Value.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = jwtSettings.Value.Issuer,
            Audience = jwtSettings.Value.Audience
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
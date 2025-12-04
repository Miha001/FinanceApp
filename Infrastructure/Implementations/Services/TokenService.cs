using Application.Abstractions.Services;
using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Application.Resources;
using Domain.Db.Entities;
using Domain.Enum;
using Domain.Models.Dto;
using Domain.Result;
using Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DAL.Implementations.Services;

public class TokenService(IMediator mediator, IOptions<JwtSettings> options) : ITokenService
{
    /// <inheritdoc/>
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(options.Value.Issuer, options.Value.Audience, claims, null,
            DateTime.UtcNow.AddDays(options.Value.AccessTokenValidityInDays),
            credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }

    /// <inheritdoc/>
    public string GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];

        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumbers);

        return Convert.ToBase64String(randomNumbers);
    }

    /// <inheritdoc/>
    public List<Claim> GetClaimsFromUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), ErrorMessages.UserNotFound);
        }
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        return claims;
    }

    /// <inheritdoc/>
    public async Task<DataResult<TokenDto>> RefreshToken(TokenDto dto)
    {
        string accessToken = dto.AccessToken;
        string refreshToken = dto.RefreshToken;

        var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
        var login = claimsPrincipal.Identity?.Name;

        var user = await mediator.Send(new GetUserWithTokenByNameQuery(login));

        if (user == null || user.UserToken.RefreshToken != refreshToken ||
            user.UserToken.RefreshTokenExpireTime <= DateTime.UtcNow)
        {
            return DataResult<TokenDto>.Failure((int)ErrorCodes.InvalidClientRequest, ErrorMessages.InvalidClientRequest);
        }

        var newClaims = GetClaimsFromUser(user);

        var newAccessToken = GenerateAccessToken(newClaims);
        var newRefreshToken = GenerateRefreshToken();

        await mediator.Send(new UpdateUserTokenCommand(user.UserToken, newRefreshToken));

        return DataResult<TokenDto>.Success(new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }

    /// <summary>
    /// Получение ClaimsPrincipal из исчезающего токена
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key)),
            ValidateLifetime = true,
            ValidAudience = options.Value.Audience,
            ValidIssuer = options.Value.Issuer
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var claimsPrincipal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new SecurityTokenException(ErrorMessages.InvalidToken);
        }

        return claimsPrincipal;
    }
}
using Finances.DAL.Implementations.Users;
using Finances.Domain.Db.Entities;
using Finances.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Finances.Tests.UnitTest;

public class TokenServiceTests
{
    [Fact]
    public void Create_ShouldGenerateValidToken_WithCorrectClaimsAndExpiration()
    {
        // Arrange
        var fakeTime = new FakeTimeProvider();

        var startTime = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
        fakeTime.SetUtcNow(startTime);

        var userName = "TestUser";
        var user = new User(userName, "some_hash");

        var jwtSettings = new JwtSettings
        {
            Key = "super_secret_key_must_be_at_least_32_chars_long_12345",
            Issuer = "MyFinanceApp",
            Audience = "MyFinanceClient",
            ExpirationInMinutes = 60
        };
        var options = Options.Create(jwtSettings);

        var service = new TokenService(options, fakeTime);
        var tokenString = service.Create(user); 

        // Act
        fakeTime.Advance(TimeSpan.FromMinutes(61));

        // Assert
        Assert.NotNull(tokenString);
        Assert.NotEmpty(tokenString);

        var handler = new JsonWebTokenHandler();
        var token = handler.ReadJsonWebToken(tokenString);

        Assert.Equal(jwtSettings.Issuer, token.Issuer);
        Assert.Contains(jwtSettings.Audience, token.Audiences);

        var nameClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var idClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        Assert.Equal(userName, nameClaim);
        Assert.Equal(user.Id.ToString(), idClaim);

        var expectedExpiration = startTime.AddMinutes(60).UtcDateTime;

        Assert.Equal(expectedExpiration, token.ValidTo, TimeSpan.FromSeconds(1));
    }
}
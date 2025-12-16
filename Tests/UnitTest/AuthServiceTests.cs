using Application.CQRS.Queries;
using Domain.Db.Entities;
using Domain.Enum;
using Domain.Models.Dto.Auth;
using Domain.Result;
using Moq;
using System.Security.Claims;
using Tests.Configurations.Fixture;

namespace Tests.UnitTest;
public class AuthServiceTests(AuthServiceFixture fixture) : IClassFixture<AuthServiceFixture>
{
    [Fact]
    public async Task Login_ShouldReturnSucess_ReturnsTokenDto()
    {
        //Arrange
        var loginDto = new LoginUserDto { Name = "testuser", Password = "passwordtest" };
        var user = new User { Name = "testuser" };

        fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserByNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        fixture.AuthValidatorMock.Setup(v => v.ValidateLogin(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(BaseResult.Success());

        //fixture.TokenServiceMock.Setup(t => t.GetClaimsFromUser(It.IsAny<User>()))
        //    .Returns(new List<Claim> { new Claim(ClaimTypes.Name, "testuser") });

        //fixture.TokenServiceMock.Setup(t => t.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>()))
        //    .Returns("access_token");

        //fixture.TokenServiceMock.Setup(t => t.GenerateRefreshToken())
        //    .Returns("refresh_token");

        //fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserTokenByUserIdQuery>(), It.IsAny<CancellationToken>()))
        //    .ReturnsAsync((UserToken)null);

        // Act
        var result = await fixture.AuthService.Login(loginDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("access_token", result.Data.JWT);
    }


    [Fact]
    public async Task Login_ShouldReturnFailure_WhenValidationFails()
    {
        // Arrange
        var loginDto = new LoginUserDto { Name = "testuser", Password = "wrongpassword" };
        var user = new User { Name = "testuser", Password = "anotherpassword" };

        var errorMessage = "Invalid password";

        fixture.MediatorMock.Setup(m => m.Send(It.IsAny<GetUserByNameQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        fixture.AuthValidatorMock.Setup(v => v.ValidateLogin(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(BaseResult.Failure((int)ErrorCodes.PasswordIsWrong, errorMessage));

        // Act
        var result = await fixture.AuthService.Login(loginDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCodes.PasswordIsWrong, result.Error.Code);
        Assert.Equal(errorMessage, result.Error.Message);
    }
}
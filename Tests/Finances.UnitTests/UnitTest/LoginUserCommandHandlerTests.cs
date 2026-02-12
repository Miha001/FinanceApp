using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Abstractions;
using Finances.Domain.Db.Entities;
using Finances.Domain.Enum;
using Moq;

namespace Tests.UnitTest;

public class LoginUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly Mock<IPasswordHasher> _hasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginUserCommandHandler _handler;

    public LoginUserCommandHandlerTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _hasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();

        _handler = new LoginUserCommandHandler(
            _repoMock.Object,
            _hasherMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserNotFound()
    {
        // Arrange
        string userName = "Unknown";
        string password = "pass";
        _repoMock.Setup(r => r.GetByName(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var command = new LoginUserCommand(userName, password);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCodes.UserNotFound, result.Error.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPasswordIsWrong()
    {
        // Arrange

        string realHash = "real_hash";
        string wrongPass = "wrong_pass";
        string userName = "User";

        var user = new User(userName, realHash);

        _repoMock.Setup(r => r.GetByName(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _hasherMock.Setup(h => h.Verify(wrongPass, realHash))
            .Returns(false);

        var command = new LoginUserCommand(userName, wrongPass);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCodes.PasswordIsWrong, result.Error.Code);
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenSuccess()
    {
        // Arrange
        string userName = "User";
        string password = "pass";
        string hash = "real_hash";

        var user = new User(userName, hash);
        var token = "generated_jwt_token";

        _repoMock.Setup(r => r.GetByName(userName, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _hasherMock.Setup(h => h.Verify(password, hash))
            .Returns(true);

        _tokenServiceMock.Setup(t => t.Create(user))
            .Returns(token);

        var command = new LoginUserCommand(userName, password);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(token, result.Data.JWT);
    }
}
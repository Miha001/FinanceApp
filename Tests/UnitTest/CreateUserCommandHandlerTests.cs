using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Abstractions;
using Finances.Domain.Db.Entities;
using Finances.Domain.Enum;
using Moq;

namespace Tests.UnitTest;


public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly Mock<IPasswordHasher> _hasherMock;
    private readonly Mock<IStateSaveChanges> _uowMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _hasherMock = new Mock<IPasswordHasher>();
        _uowMock = new Mock<IStateSaveChanges>();

        _handler = new CreateUserCommandHandler(
            _repoMock.Object,
            _hasherMock.Object,
            _uowMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserExists()
    {
        // Arrange
        var command = new CreateUserCommand("Ivan", "password123");

        _repoMock.Setup(r => r.ExistsByName(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal((int)ErrorCodes.UserAlreadyExists, result.Error.Code);

        _repoMock.Verify(r => r.Create(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenSuccess()
    {
        // Arrange
        var command = new CreateUserCommand("NewUser", "pass");
        var hashedPassword = "hashed_pass";

        _repoMock.Setup(r => r.ExistsByName(command.Name, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _hasherMock.Setup(h => h.Hash(command.Password))
            .Returns(hashedPassword);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("NewUser", result.Data.Name);

        _repoMock.Verify(r => r.Create(
            It.Is<User>(u => u.Name == "NewUser" && u.Password == hashedPassword),
            It.IsAny<CancellationToken>()), Times.Once);

        _uowMock.Verify(u => u.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
    }
}

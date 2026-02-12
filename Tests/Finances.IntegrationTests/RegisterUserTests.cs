using Finances.Application.Abstractions.Users.Commands;
using Finances.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace Finances.UsersIntegrationTests;

public class RegisterUserTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Handle_ShouldCreateUser_WhenCommandIsValid()
    {
        // Arrange
        var command = new RegisterUserCommand("new_user_test", "StrongPass123!");

        // Act
        var result = await Sender.Send(command);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();

        result.Data.Should().NotBeNull();
        result.Data.Name.Should().Be(command.Name);

        var userInDb = await DataContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Name == command.Name);

        userInDb.Should().NotBeNull();
        userInDb!.Name.Should().Be(command.Name);
        userInDb.Password.Should().NotBe(command.Password);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenUserAlreadyExists()
    {
        // Arrange
        var command = new RegisterUserCommand("duplicate_user", "AnyPassword123");

        // Сначала создаем пользователя (первый вызов)
        var firstCreate = await Sender.Send(command);
        firstCreate.IsSuccess.Should().BeTrue();

        // Act
        var secondResult = await Sender.Send(command);

        // Assert
        secondResult.IsSuccess.Should().BeFalse();

        secondResult.Error.Should().NotBeNull();
        secondResult.Error.Code.Should().Be((int)ErrorCodes.UserAlreadyExists);

        secondResult.Data.Should().BeNull();

        var count = await DataContext.Users.CountAsync(u => u.Name == command.Name);
        count.Should().Be(1);
    }
}
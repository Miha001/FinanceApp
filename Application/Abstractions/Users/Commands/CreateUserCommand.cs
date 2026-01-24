using Finances.Application.Abstractions.Shared;
using Finances.Application.Abstractions.Users;
using Finances.Domain.Db.Entities;
using MediatR;

namespace Finances.Application.Abstractions.Users.Commands;

/// <summary>
/// Создание пользователя при регистрации.
/// </summary>
/// <param name="Name"></param>
/// <param name="Password"></param>
public sealed record CreateUserCommand(string Name, string Password) : IRequest<User>;

public class CreateUserCommandHandler(IBaseRepository<User> userRepository, IPasswordHasher passwordHasher, IStateSaveChanges stateSaveChanges) : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        var user = new User {
            Name = request.Name,
            Password = passwordHasher.Hash(request.Password),
        };

        await userRepository.CreateAsync(user);
        await stateSaveChanges.SaveChangesAsync(cancellationToken);
        return user;
    }
}
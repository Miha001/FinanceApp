using Finances.Application.Abstractions.Shared;
using Finances.Domain.Abstractions;
using Finances.Domain.Db.Entities;
using Finances.Domain.Enum;
using Finances.Domain.Resources;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Users.Commands;

/// <summary>
/// Создание пользователя при регистрации.
/// </summary>
/// <param name="Name"></param>
/// <param name="Password"></param>
public sealed record CreateUserCommand(string Name, string Password) : IRequest<DataResult<User>>;

public class CreateUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IStateSaveChanges stateSaveChanges) : IRequestHandler<CreateUserCommand, DataResult<User>>
{
    public async Task<DataResult<User>> Handle(CreateUserCommand request, CancellationToken ct)
    {
        if (await userRepository.ExistsByName(request.Name, ct))
        {
            return DataResult<User>.Failure((int)ErrorCodes.UserAlreadyExists, ErrorMessages.UserAlreadyExists);
        }

        var user = new User(request.Name, passwordHasher.Hash(request.Password));

        await userRepository.Create(user, ct);
        await stateSaveChanges.SaveChanges(ct);

        return DataResult<User>.Success(user);
    }
}
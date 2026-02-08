using Finances.Application.Abstractions.Shared;
using Finances.Domain.Abstractions;
using Finances.Domain.Db.Entities;
using Finances.Domain.Enum;
using Finances.Domain.Models.Dto.Auth;
using Finances.Domain.Resources;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Users.Commands;

public record RegisterUserCommand(string Name, string Password) : IRequest<DataResult<UserNameDto>>;

public class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IStateSaveChanges stateSaveChanges)
    : IRequestHandler<RegisterUserCommand, DataResult<UserNameDto>>
{
    public async Task<DataResult<UserNameDto>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        if (await userRepository.ExistsByName(request.Name, ct))
        {
            return DataResult<UserNameDto>.Failure((int)ErrorCodes.UserAlreadyExists, ErrorMessages.UserAlreadyExists);
        }

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = new User(request.Name, passwordHash);

        await userRepository.Create(user, ct);
        await stateSaveChanges.SaveChanges(ct);

        return DataResult<UserNameDto>.Success(new UserNameDto(user.Name));
    }
}
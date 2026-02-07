using Finances.Application.Abstractions.Shared;
using Finances.Domain.Abstractions;
using Finances.Domain.Enum;
using Finances.Domain.Models.Dto;
using Finances.Domain.Resources;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Users.Commands;

public record LoginUserCommand(string Name, string Password) : IRequest<DataResult<TokenDto>>;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
    : IRequestHandler<LoginUserCommand, DataResult<TokenDto>>
{
    public async Task<DataResult<TokenDto>> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByName(request.Name, ct);

        if (user == null)
        {
            return DataResult<TokenDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessages.UserNotFound);
        }

        if (!passwordHasher.Verify(request.Password, user.Password))
        {
            return DataResult<TokenDto>.Failure((int)ErrorCodes.PasswordIsWrong, ErrorMessages.PasswordIsWrong);
        }

        var token = tokenService.Create(user);

        return DataResult<TokenDto>.Success(new TokenDto { JWT = token });
    }
}
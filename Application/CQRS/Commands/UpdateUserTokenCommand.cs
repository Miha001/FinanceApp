using Application.Abstractions.Repositories;
using Domain.Db.Entities;
using MediatR;

namespace Application.CQRS.Commands;


/// <summary>
/// Обновление Refresh-токена пользователя.
/// </summary>
/// <param name="UserToken"></param>
/// <param name="RefreshToken"></param>
public sealed record UpdateUserTokenCommand(UserToken UserToken, string RefreshToken) : IRequest;

public class UpdateUserTokenHandler(IBaseRepository<UserToken> userTokenRepository) : IRequestHandler<UpdateUserTokenCommand>
{
    public async Task Handle(UpdateUserTokenCommand request, CancellationToken cancellationToken = default)
    {
        var userToken = request.UserToken;

        userToken.RefreshToken = request.RefreshToken;
        userToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7); //TODO: вынести в конфигурацию

        userTokenRepository.Update(userToken);

        await userTokenRepository.SaveChangesAsync(cancellationToken);
    }
}

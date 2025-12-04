using Application.Abstractions.Repositories;
using Domain.Db.Entities;
using MediatR;

namespace Application.CQRS.Commands;
public sealed record CreateUserTokenCommand(Guid UserId, string RefreshToken, int RefreshTokenValidityInDays) : IRequest;

public class CreateUserTokenHandler(IBaseRepository<UserToken> userTokenRepository) : IRequestHandler<CreateUserTokenCommand>
{
    public async Task Handle(CreateUserTokenCommand request, CancellationToken cancellationToken)
    {
        var userToken = new UserToken()
        {
            UserId = request.UserId,
            RefreshToken = request.RefreshToken,
            RefreshTokenExpireTime = DateTime.UtcNow.AddDays(request.RefreshTokenValidityInDays)
        };

        await userTokenRepository.CreateAsync(userToken);

        await userTokenRepository.SaveChangesAsync(cancellationToken);
    }
}
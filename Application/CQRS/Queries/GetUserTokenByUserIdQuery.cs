using Application.Abstractions.Repositories;
using Domain.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries;

/// <summary>
/// Получение токена пользователя по идентификатору пользователя.
/// </summary>
/// <param name="UserId"></param>
public sealed record GetUserTokenByUserIdQuery(Guid UserId, bool AsNoTracking = false) : IRequest<UserToken>;

public class GetUserTokenByUserIdQueryHandler(IBaseRepository<UserToken> userTokenRepository)
    : IRequestHandler<GetUserTokenByUserIdQuery, UserToken>
{
    public async Task<UserToken> Handle(GetUserTokenByUserIdQuery request, CancellationToken cancellationToken)
    {
        return await userTokenRepository
            .Query(request.AsNoTracking)
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
    }
}
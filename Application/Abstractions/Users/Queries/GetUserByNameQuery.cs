using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Finances.Application.Abstractions.Users.Queries;
public sealed record GetUserByNameQuery(string Name, bool AsNoTracking = false) : IRequest<User>;

public class GetUserByNameQueryHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserByNameQuery, User?>
{
    public async Task<User?> Handle(GetUserByNameQuery request, CancellationToken ct)
    {
        return await userRepository
            .Query(request.AsNoTracking)
            .FirstOrDefaultAsync(x => x.Name == request.Name, ct);
    }
}
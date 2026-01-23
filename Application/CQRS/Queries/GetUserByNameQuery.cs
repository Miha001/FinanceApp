using Finances.Application.Abstractions.Repositories;
using Finances.Domain.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Finances.Application.CQRS.Queries;
public sealed record GetUserByNameQuery(string Name, bool AsNoTracking = false) : IRequest<User>;

public class GetUserByNameQueryHandler(IBaseRepository<User> userRepository) : IRequestHandler<GetUserByNameQuery, User>
{
    public async Task<User> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
    {
        return await userRepository
            .Query(request.AsNoTracking)
            .FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
    }
}
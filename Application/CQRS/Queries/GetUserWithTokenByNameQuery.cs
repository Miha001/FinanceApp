using Application.Abstractions.Repositories;
using Domain.Db.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries;
public sealed record GetUserWithTokenByNameQuery(string Name) : IRequest<User>;

public class GetUserWithTokenByNameHandler(IBaseRepository<User> usersRepository) : IRequestHandler<GetUserWithTokenByNameQuery, User>
{
    public async Task<User> Handle(GetUserWithTokenByNameQuery request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.Query(asNoTracking: false)
            .Include(x => x.UserToken)
            .FirstOrDefaultAsync(x => x.Name == request.Name);
        
        return user;
    }
}
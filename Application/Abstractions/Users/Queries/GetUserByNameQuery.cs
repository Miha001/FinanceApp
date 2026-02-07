using Finances.Domain.Db.Entities;
using MediatR;

namespace Finances.Application.Abstractions.Users.Queries;
public sealed record GetUserByNameQuery(string Name, bool AsNoTracking = false) : IRequest<User>;

public class GetUserByNameQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByNameQuery, User?>
{
    public async Task<User?> Handle(GetUserByNameQuery request, CancellationToken ct)
    {
        return await userRepository.GetByName(request.Name, ct);
    }
}
using Finances.Application.Abstractions.Users;
using Finances.DAL.Implementations.Shared;
using Finances.Domain.Db.Entities;
using Finances.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Finances.DAL.Implementations.Users;

public class UserRepository(DataContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByName(string name, CancellationToken ct = default)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Name == name, ct);
    }

    public async Task<bool> ExistsByName(string name, CancellationToken ct = default)
    {
        return await context.Users
            .AnyAsync(u => u.Name == name, ct);
    }
}
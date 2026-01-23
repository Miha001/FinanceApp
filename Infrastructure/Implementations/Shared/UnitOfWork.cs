using Finances.Application.Abstractions.Shared;
using Finances.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Finances.DAL.Implementations.Shared;
public class UnitOfWork(DataContext dbContext) : IUnitOfWork
{
    /// <inheritdoc/>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        return await dbContext.Database.BeginTransactionAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await dbContext.SaveChangesAsync(ct);
    }
}
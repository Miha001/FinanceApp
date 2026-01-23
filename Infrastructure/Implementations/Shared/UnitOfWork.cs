using Finances.Application.Abstractions.Shared;
using Finances.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Finances.DAL.Implementations.Shared;
public class UnitOfWork(DataContext dbContext) : IUnitOfWork
{
    /// <inheritdoc/>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default)
    {
        return await dbContext.Database.BeginTransactionAsync(token);
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
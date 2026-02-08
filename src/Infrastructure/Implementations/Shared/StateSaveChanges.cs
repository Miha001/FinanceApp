using Finances.Application.Abstractions.Shared;
using Finances.Infrastructure.Db.Context;

namespace Finances.DAL.Implementations.Shared;
public class StateSaveChanges(DataContext dbContext) : IStateSaveChanges
{
    /// <inheritdoc/>
    public async Task SaveChanges(CancellationToken ct = default)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}
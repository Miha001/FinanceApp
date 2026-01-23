using Finances.Application.Abstractions.Repositories;
using Finances.DAL.Implementations.Shared;
using Finances.Domain.Db.Entities;
using Finances.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Finances.DAL.Implementations.Carrencies;
public class CurrenciesRepository(DataContext dataContext) : BaseRepository<Currency>(dataContext), ICurrenciesRepository
{
    ///<inheritdoc/>
    public async Task AddRangeAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(currencies, cancellationToken);
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    ///<inheritdoc/>
    public void UpdateRange(IEnumerable<Currency> currencies)
    {
        _dbSet.UpdateRange(currencies);
    }
}
using Finances.Application.Abstractions.Currencies;
using Finances.DAL.Implementations.Shared;
using Finances.Domain.Db.Entities;
using Finances.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Finances.DAL.Implementations.Carrencies;
public class CurrenciesRepository(DataContext dataContext) : BaseRepository<Currency>(dataContext), ICurrenciesRepository
{
    ///<inheritdoc/>
    public async Task AddRange(IEnumerable<Currency> currencies, CancellationToken ct = default)
    {
        await _dbSet.AddRangeAsync(currencies, ct);
    }

    ///<inheritdoc/>
    public async Task<IEnumerable<Currency>> GetAll(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }

    ///<inheritdoc/>
    public void UpdateRange(IEnumerable<Currency> currencies)
    {
        _dbSet.UpdateRange(currencies);
    }
}
using Application.Abstractions.Repositories;
using Domain.Db.Entities;
using Infrastructure.Db.Context;
using Infrastructure.Implementations.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementations.Repositories;
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
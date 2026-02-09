using Finances.Application.Abstractions.Currencies;
using Finances.DAL.Extensions;
using Finances.DAL.Implementations.Shared;
using Finances.Domain.Db.Entities;
using Finances.Domain.Models;
using Finances.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Finances.DAL.Implementations.Carrencies;
public class CurrenciesRepository(DataContext dataContext) : BaseRepository<Currency>(dataContext), ICurrenciesRepository
{
    ///<inheritdoc/>
    public async Task<IReadOnlyCollection<Currency>> GetFavoriteByUserId(Guid userId, CancellationToken ct = default, Pagination ? pagination = null)
    {
        var currencies = await _dbSet
            .AsNoTracking()
            .Where(c => c.UserCurrencies.Any(uc => uc.UserId == userId))
            .SkipAndTake(pagination)
            .ToListAsync(ct);

        return currencies;
    }
}
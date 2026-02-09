using Finances.Domain.Models;

namespace Finances.DAL.Extensions;
public static class QueryExtesions
{
    /// <summary>
    /// Применить пагинацию
    /// </summary>
    /// <param name="query"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public static IQueryable<TEntity> SkipAndTake<TEntity>(this IQueryable<TEntity> query, Pagination? p)
    {
        return p is null ? query :
           query
            .Skip((p.PageNumber - 1) * p.PageSize)
            .Take(p.PageSize);
    }
}
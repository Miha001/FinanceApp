using Finances.Application.Abstractions.Shared;
using Finances.Infrastructure.Db.Context;
using Microsoft.EntityFrameworkCore;

namespace Finances.DAL.Implementations.Shared;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    private readonly DataContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(DataContext dataContext)
    {
        _dbContext = dataContext;
        _dbSet = dataContext.Set<TEntity>();
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> Query(bool asNoTracking = false)
    {
        var query = _dbSet.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        return query;
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _dbContext.SaveChangesAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        await _dbSet.AddAsync(entity);

        return entity;
    }

    /// <inheritdoc/>
    public TEntity Update(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        _dbSet.Update(entity);

        return entity;
    }

    private void ValidateEntityOnNull(TEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity), "Entity is null");
    }
}
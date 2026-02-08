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
    public async Task<IReadOnlyCollection<TEntity>> GetAll(bool asNoTracking = false, CancellationToken ct = default)
    {
        var query = _dbSet.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _dbContext.SaveChangesAsync(ct);
    }

    /// <inheritdoc/>
    public async Task<TEntity> Create(TEntity entity, CancellationToken ct = default)
    {
        ValidateEntityOnNull(entity);

        await _dbSet.AddAsync(entity, ct);

        return entity;
    }

    /// <summary>
    /// Проверка сущности на null
    /// </summary>
    /// <param name="entity"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ValidateEntityOnNull(TEntity entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity), "Entity is null");
    }
}
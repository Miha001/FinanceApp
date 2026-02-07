namespace Finances.Application.Abstractions.Shared;

/// <summary>
/// Абстракция для взаимодействия с базой данных, через DbContext.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity>
{
    /// <summary>
    /// Возвращает все сущности как IQueryable.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<TEntity>> GetAll(bool asNoTracking = false, CancellationToken ct = default);

    /// <summary>
    /// Создает новую сущность.
    /// </summary>
    /// <param name="entity">Сущность для создания.</param>
    /// <returns>Созданная сущность.</returns>
    Task<TEntity> Create(TEntity entity, CancellationToken ct);
}
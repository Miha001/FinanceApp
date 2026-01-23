namespace Finances.Application.Abstractions.Shared;

/// <summary>
/// Абстракция для взаимодействия с базой данных, через DbContext.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity> : IStateSaveChanges
{
    /// <summary>
    /// Возвращает все сущности как IQueryable.
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> Query(bool asNoTracking = false);

    /// <summary>
    /// Создает новую сущность.
    /// </summary>
    /// <param name="entity">Сущность для создания.</param>
    /// <returns>Созданная сущность.</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Обновляет сущность.
    /// </summary>
    /// <param name="entity">Сущность для обновления.</param>
    /// <returns>Обновленная сущность.</returns>
    TEntity Update(TEntity entity);
}
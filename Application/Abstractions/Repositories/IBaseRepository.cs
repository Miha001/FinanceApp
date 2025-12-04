namespace Application.Abstractions.Repositories;

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

    /// <summary>
    /// Удаляет сущность.
    /// </summary>
    /// <param name="entity">Сущность для удаления.</param>
    void Remove(TEntity entity);
}
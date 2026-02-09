using System.Linq.Expressions;

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

    /// <summary>
    /// Проверяет существует ли метод по условию(предикату).
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="ct"></param>
    /// <returns>Boolean</returns>
    Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
}
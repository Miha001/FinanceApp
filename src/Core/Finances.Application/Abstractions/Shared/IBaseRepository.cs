using Finances.Domain.Db.Entities;
using Finances.Domain.Models;
using System.Linq.Expressions;

namespace Finances.Application.Abstractions.Shared;

/// <summary>
/// Абстракция для взаимодействия с базой данных, через DbContext.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity>
{
    /// <summary>
    /// Возвращает постраничный список сущностей
    /// </summary>
    Task<IReadOnlyCollection<TEntity>> GetAllPaged(
        bool asNoTracking = false,
        CancellationToken ct = default,
        Pagination? pagination = null);

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

    /// <summary>
    /// Добавить список сущностей
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task AddRange(IEnumerable<TEntity> entities, CancellationToken ct = default);

    /// <summary>
    /// Получить полное количество сущностей по условию(предикату)
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<int> GetCount(CancellationToken ct = default, Expression<Func<TEntity, bool>>? predicate = null);
}
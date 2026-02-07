using Finances.Domain.Db.Entities;

namespace Finances.Application.Abstractions.Currencies;

/// <summary>
/// Абстракция для взаимодействия с курсом валют через DbContext
/// </summary>
public interface ICurrenciesRepository
{
    /// <summary>
    ///  Получить все курсы валют
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Currency>> GetAll(CancellationToken ct = default);

    /// <summary>
    /// Получить избранные валюты пользователя по userId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Currency>> GetByUserId(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Добавить список курсов валют
    /// </summary>
    /// <param name="currencies"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task AddRange(IEnumerable<Currency> currencies, CancellationToken ct = default);
}
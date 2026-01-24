using Finances.Application.Abstractions.Shared;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Currency>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить список курсов валют
    /// </summary>
    /// <param name="currencies"></param>
    /// <returns></returns>
    void UpdateRange(IEnumerable<Currency> currencies);

    /// <summary>
    /// Добавить список курсов валют
    /// </summary>
    /// <param name="currencies"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddRangeAsync(IEnumerable<Currency> currencies, CancellationToken cancellationToken = default);
}
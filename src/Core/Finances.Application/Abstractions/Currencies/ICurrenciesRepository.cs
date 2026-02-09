using Finances.Domain.Db.Entities;
using Finances.Domain.Models;

namespace Finances.Application.Abstractions.Currencies;

/// <summary>
/// Абстракция для взаимодействия с курсом валют через DbContext
/// </summary>
public interface ICurrenciesRepository
{
    /// <summary>
    /// Получить избранные валюты пользователя по userId
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Currency>> GetFavoriteByUserId(Guid userId, CancellationToken ct = default, Pagination? pagination = null);
}
using Microsoft.Extensions.Caching.Distributed;

namespace Application.Abstractions.Services;

/// <summary>
/// Сервис для работы с кэшированием
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Получить объект из кэша по ключу
    /// </summary>
    /// <typeparam name="T">Тип объекта, который получаем из кэша.</typeparam>
    /// <param name="key">Ключ кэша.</param>
    /// <returns></returns>
    Task<T> GetObjectAsync<T>(string key) where T : class;

    /// <summary>
    /// Положить объект в хранилище кэша.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Ключ кэша.</param>
    /// <param name="obj">Объект, который кладём в кэш.</param>
    /// <param name="options">Настройки кэширования.</param>
    /// <returns></returns>
    Task SetObjectAsync<T>(string key, T obj, DistributedCacheEntryOptions? options = null) where T : class;

    /// <summary>
    /// Убрать объект из кэша по ключу.
    /// </summary>
    /// <param name="key">Ключ кэша.</param>
    /// <returns></returns>
    Task RemoveAsync(string key);
}
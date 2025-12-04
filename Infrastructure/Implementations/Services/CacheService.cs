using Application.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace DAL.Implementations.Services;

public class CacheService(IDistributedCache cache) : ICacheService
{
    /// <inheritdoc/>
    public async Task SetObjectAsync<T>(string key, T obj, DistributedCacheEntryOptions options = null) where T : class
    {
        var data = JsonSerializer.SerializeToUtf8Bytes(obj);
        if (data.Length > 0)
        {
            await cache.SetAsync(key, data, options ?? new DistributedCacheEntryOptions()
            { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) });
        }
    }

    /// <inheritdoc/>
    public async Task RemoveAsync(string key)
    {
        if (!string.IsNullOrEmpty(key))
        {
            var value = await cache.GetStringAsync(key);

            if (value != null)
            {
                await cache.RemoveAsync(key);
            }
        }
    }

    /// <inheritdoc/>
    public async Task<T> GetObjectAsync<T>(string key) where T : class
    {
        var data = await cache.GetAsync(key);
        return data != null ? JsonSerializer.Deserialize<T>(data) : default;
    }
}

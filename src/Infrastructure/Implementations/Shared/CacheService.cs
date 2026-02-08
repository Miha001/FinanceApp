using Finances.Application.Abstractions.Shared;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Finances.DAL.Implementations.Shared;

public class CacheService(IDistributedCache cache) : ICacheService
{
    /// <inheritdoc/>
    public async Task SetObject<T>(string key, T obj, DistributedCacheEntryOptions options = null, CancellationToken ct = default) where T : class
    {
        var data = JsonSerializer.SerializeToUtf8Bytes(obj);
        if (data.Length > 0)
        {
            await cache.SetAsync(key, data, options ?? new DistributedCacheEntryOptions()
            { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) }, ct);
        }
    }

    /// <inheritdoc/>
    public async Task<T> GetObject<T>(string key) where T : class
    {
        var data = await cache.GetAsync(key);
        return data != null ? JsonSerializer.Deserialize<T>(data) : default;
    }
}
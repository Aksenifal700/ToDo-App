using TodoApp.Interfaces.IServices;

namespace TodoApp.DataAccess.Caching;

public class CachedQueryService : ICachedQueryService
{
    private readonly ICacheService _cache;
    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(5);

    public CachedQueryService(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetOrSetAsync<T>(string cacheKey, Func<Task<T?>> fetchFromSource, TimeSpan? expiry = null)
    {
        var cached = await _cache.GetAsync<T>(cacheKey);
        if (cached is not null)
            return cached;

        var freshData = await fetchFromSource();
        if (freshData is not null)
            await _cache.SetAsync(cacheKey, freshData, expiry ?? DefaultExpiry);

        return freshData;
    }

    public async Task InvalidateAsync(params string[] cacheKeys)
    {
        foreach (var key in cacheKeys)
            await _cache.RemoveAsync(key);
    }
}
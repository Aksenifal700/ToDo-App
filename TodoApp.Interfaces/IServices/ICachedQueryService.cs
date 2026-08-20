namespace TodoApp.Interfaces.IServices;

public interface ICachedQueryService
{
    Task<T?> GetOrSetAsync<T>(string cacheKey, Func<Task<T?>> fetchFromSource, TimeSpan? expiry = null);
    Task InvalidateAsync(params string[] cacheKeys);
}
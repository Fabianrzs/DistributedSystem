namespace Reports.Application.Abstractions.Cache;

public interface IDistributedCacheService
{
    Task SetAsync(string cacheKey, byte[] data, TimeSpan? expiration = null);
    Task<byte[]> GetAsync(string cacheKey);
    Task RemoveAsync(string cacheKey);
}

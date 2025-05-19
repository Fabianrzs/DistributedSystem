using Reports.Application.Abstractions.Cache;
using StackExchange.Redis;

namespace Reports.Infrastructure.Implementations.Cache;

public class DistributedCacheService(IConnectionMultiplexer redisConnection) : IDistributedCacheService
{
    public async Task SetAsync(string cacheKey, byte[] data, TimeSpan? expiration = null)
    {
        IDatabase db = redisConnection.GetDatabase();
        var options = new TimeSpan?(
            expiration ?? TimeSpan.FromMinutes(60));
        await db.StringSetAsync(cacheKey, data, options);
    }

    public async Task<byte[]> GetAsync(string cacheKey)
    {
        IDatabase db = redisConnection.GetDatabase();
        return await db.StringGetAsync(cacheKey);
    }

    public async Task RemoveAsync(string cacheKey)
    {
        IDatabase db = redisConnection.GetDatabase();
        await db.KeyDeleteAsync(cacheKey);
    }
}

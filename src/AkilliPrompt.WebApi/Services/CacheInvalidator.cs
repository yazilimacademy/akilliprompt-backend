using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace AkilliPrompt.WebApi.Services;

public class CacheInvalidator
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CacheInvalidator> _logger;
    private readonly IDatabase _redisDb;

    public CacheInvalidator(
        IDistributedCache cache,
        IConnectionMultiplexer redis,
        ILogger<CacheInvalidator> logger)
    {
        _cache = cache;
        _redisDb = redis.GetDatabase();
        _logger = logger;
    }

    public async Task InvalidateAsync(string cacheKey, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        _logger.LogInformation($"[Cache Invalidate] Key: {cacheKey}");
    }

    public async Task InvalidateGroupAsync(string cacheGroup, CancellationToken cancellationToken)
    {
        var groupSetKey = $"Group:{cacheGroup}";

        var cacheKeys = await _redisDb.SetMembersAsync(groupSetKey);

        foreach (var key in cacheKeys)
        {
            await _cache.RemoveAsync(key, cancellationToken);

            _logger.LogInformation($"[Cache Invalidate] Group: {cacheGroup}, Key: {key}");
        }

        // Remove the group set after invalidation
        await _redisDb.KeyDeleteAsync(groupSetKey);
    }
}

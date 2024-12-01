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

    // Muhammet bizi uyardi. Pattern'e uygun key'leri silen bir script yazmanin daha iyi oldugun iletti.
    // Bu duruma uygulamayi deploy ettikten sonra bir bakalim.
    public async Task InvalidateGroupAsync(string cacheGroup, CancellationToken cancellationToken)
{
    try
    {
        ArgumentException.ThrowIfNullOrEmpty(cacheGroup);
        
        var groupSetKey = CreateGroupKey(cacheGroup);

        var cacheKeys = await _redisDb.SetMembersAsync(groupSetKey);

        // Use parallel processing for better performance with large sets
        var tasks = cacheKeys.Select(async key =>
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken);

                _logger.LogInformation("Cache key {Key} from group {Group} invalidated", key, cacheGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to invalidate cache key {Key} from group {Group}", key, cacheGroup);
            }
        });

        await Task.WhenAll(tasks);
        await _redisDb.KeyDeleteAsync(groupSetKey);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to invalidate cache group {Group}", cacheGroup);
        throw;
    }
}

// Extract string literals to constants
private const string GroupKeyPrefix = "Group:";
private static string CreateGroupKey(string group) => $"{GroupKeyPrefix}{group}";
}

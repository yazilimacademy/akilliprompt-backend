using System;
using Microsoft.Extensions.Caching.Distributed;

namespace AkilliPrompt.WebApi.Services;

public class CacheInvalidator : ICacheInvalidator
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CacheInvalidator> _logger;

    public CacheInvalidator(IDistributedCache cache, ILogger<CacheInvalidator> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task InvalidateAsync(string cacheKey, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(cacheKey, cancellationToken);

        _logger.LogInformation($"Cache invalidated for key: {cacheKey}");
    }
}

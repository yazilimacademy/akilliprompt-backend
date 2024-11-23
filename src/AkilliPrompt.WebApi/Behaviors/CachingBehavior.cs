using System.Text.Json;
using AkilliPrompt.Domain.Common;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace AkilliPrompt.WebApi.Behaviors;

public sealed class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;
    public CachingBehavior(IDistributedCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Only cache queries (requests that implement ICacheable)
        if (request is not ICacheable cacheableRequest)
            return await next();

        var cacheKey = cacheableRequest.CacheKey;

        var cachedData = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (!string.IsNullOrEmpty(cachedData))
        {
            _logger.LogInformation($"Cache hit for key: {cacheKey}");
            return JsonSerializer.Deserialize<TResponse>(cachedData);
        }

        _logger.LogInformation($"Cache miss for key: {cacheKey}. Fetching from database.");
        var response = await next();

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60), // Adjust as needed
            SlidingExpiration = TimeSpan.FromMinutes(30)
        };

        var serializedResponse = JsonSerializer.Serialize(response);

        await _cache.SetStringAsync(cacheKey, serializedResponse, cacheOptions, cancellationToken);

        return response;
    }
}

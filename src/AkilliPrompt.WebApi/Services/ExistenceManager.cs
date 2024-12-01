using AkilliPrompt.Domain.Common;
using AkilliPrompt.Persistence.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace AkilliPrompt.WebApi.Services;

/// <summary>
/// Generic service to check existence of entities with caching.
/// </summary>
/// <typeparam name="TEntity">Type of the entity.</typeparam>
public class ExistenceManager<TEntity> : IExistenceService<TEntity>
    where TEntity : EntityBase
{
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ExistenceManager<TEntity>> _logger;
    private const string CacheKeyPrefix = "EntityExists:";

    public ExistenceManager(
        ApplicationDbContext context,
        IDistributedCache cache,
        ILogger<ExistenceManager<TEntity>> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = GenerateCacheKey(id);

        try
        {
            // Attempt to retrieve from cache
            var cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedValue))
            {
                if (bool.TryParse(cachedValue, out bool exists))
                {
                    _logger.LogInformation($"[Cache Hit] {typeof(TEntity).Name} ID: {id}");
                    return exists;
                }
            }

            _logger.LogInformation($"[Cache Miss] {typeof(TEntity).Name} ID: {id}. Querying database.");

            // Query the database
            bool existsInDb = await _context.Set<TEntity>()
            .AnyAsync(e => e.Id == id, cancellationToken);

            // Cache the result
            await SetExistenceAsync(id, existsInDb, cancellationToken);

            return existsInDb;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error checking existence of {typeof(TEntity).Name} ID: {id}");
            // Fallback to database query if cache fails
            return await _context.Set<TEntity>()
            .AnyAsync(e => e.Id == id, cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task SetExistenceAsync(Guid id, bool exists, CancellationToken cancellationToken)
    {
        var cacheKey = GenerateCacheKey(id);

        var serializedValue = JsonSerializer.Serialize(exists);

        try
        {
            await _cache.SetStringAsync(cacheKey, serializedValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            }, cancellationToken);

            _logger.LogInformation($"[Cache Set] {typeof(TEntity).Name} ID: {id} exists: {exists}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error setting cache for {typeof(TEntity).Name} ID: {id}");
        }
    }

    /// <inheritdoc />
    public async Task RemoveExistenceAsync(Guid id, CancellationToken cancellationToken)
    {
        var cacheKey = GenerateCacheKey(id);

        try
        {
            await _cache.RemoveAsync(cacheKey, cancellationToken);

            _logger.LogInformation($"[Cache Removed] {typeof(TEntity).Name} ID: {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error removing cache for {typeof(TEntity).Name} ID: {id}");
        }
    }

    /// <summary>
    /// Generates a cache key based on the entity type and its identifier.
    /// </summary>
    /// <param name="id">Identifier of the entity.</param>
    /// <returns>Generated cache key.</returns>
    private string GenerateCacheKey(Guid id) => $"{CacheKeyPrefix}{typeof(TEntity).Name}:{id}";
}


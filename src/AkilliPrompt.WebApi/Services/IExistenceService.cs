using AkilliPrompt.Domain.Common;

namespace AkilliPrompt.WebApi.Services;

/// <summary>
/// Provides methods to check the existence of entities with caching.
/// </summary>
/// <typeparam name="TEntity">Type of the entity.</typeparam>
public interface IExistenceService<TEntity>
    where TEntity : EntityBase
{
    /// <summary>
    /// Checks if an entity with the specified ID exists.
    /// </summary>
    /// <param name="id">Identifier of the entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Adds or updates the existence status of an entity in the cache.
    /// </summary>
    /// <param name="id">Identifier of the entity.</param>
    /// <param name="exists">Existence status.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetExistenceAsync(Guid id, bool exists, CancellationToken cancellationToken);

    /// <summary>
    /// Removes the existence status of an entity from the cache.
    /// </summary>
    /// <param name="id">Identifier of the entity.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RemoveExistenceAsync(Guid id, CancellationToken cancellationToken);
}


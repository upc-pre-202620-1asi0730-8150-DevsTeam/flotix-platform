namespace FlotixPlatform.Api.Shared.Domain.Repositories;

/// <summary>
///     Defines the common repository operations for an entity type.
/// </summary>
/// <typeparam name="TEntity">The entity type managed by the repository.</typeparam>
public interface IBaseRepository<TEntity>
{
    /// <summary>
    ///     Adds an entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to find.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task containing the entity if found, or null otherwise.</returns>
    Task<TEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Marks an entity for update.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);

    /// <summary>
    ///     Removes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    void Remove(TEntity entity);

    /// <summary>
    ///     Lists all entities in the repository.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task containing the collection of entities.</returns>
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);
}
namespace FlotixPlatform.Api.Shared.Domain.Repositories;

/// <summary>
///     Defines a unit of work for committing changes made through repositories.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Commits the pending changes in the unit of work.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CompleteAsync(CancellationToken cancellationToken = default);
}
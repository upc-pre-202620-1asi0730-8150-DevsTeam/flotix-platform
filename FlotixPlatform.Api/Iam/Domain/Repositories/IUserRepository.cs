using FlotixPlatform.Api.Iam.Domain.Model.Aggregates;
using FlotixPlatform.Api.Iam.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Iam.Domain.Repositories;

/**
 * <summary>
 *     The user repository
 * </summary>
 * <remarks>
 *     This repository is used to manage users. IAM users use their own integer identifier,
 *     so this contract is intentionally independent of the GUID-based IBaseRepository.
 * </remarks>
 */
public interface IUserRepository
{
    /**
     * <summary>
     *     Add a user to the repository
     * </summary>
     * <param name="user">The user to add</param>
     * <param name="cancellationToken">The cancellation token</param>
     */
    Task AddAsync(User user, CancellationToken cancellationToken);

    /**
     * <summary>
     *     Find a user by username
     * </summary>
     * <param name="username">The username to search</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The user</returns>
     */
    Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken);

    /**
     * <summary>
     *     Find a user by username without change tracking
     * </summary>
     * <param name="username">The username to search</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The user</returns>
     */
    Task<User?> FindByUsernameAsNoTrackingAsync(string username, CancellationToken cancellationToken);

    /**
     * <summary>
     *     Find a user by id without change tracking
     * </summary>
     * <param name="id">The user id to search</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The user</returns>
     */
    Task<User?> FindByIdAsNoTrackingAsync(UserId id, CancellationToken cancellationToken);

    /**
     * <summary>
     *     List all users without change tracking
     * </summary>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>The users</returns>
     */
    Task<IEnumerable<User>> ListAsNoTrackingAsync(CancellationToken cancellationToken);

    /**
     * <summary>
     *     Check if a user exists by username
     * </summary>
     * <param name="username">The username to search</param>
     * <param name="cancellationToken">The cancellation token</param>
     * <returns>True if the user exists, false otherwise</returns>
     */
    Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken);

    void Update(User user);

    void Remove(User user);
}

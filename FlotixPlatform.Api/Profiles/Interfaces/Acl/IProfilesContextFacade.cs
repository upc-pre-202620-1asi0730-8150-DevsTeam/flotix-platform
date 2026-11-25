namespace FlotixPlatform.Api.Profiles.Interfaces.Acl;

/// <summary>
///     Facade for the profiles context
/// </summary>
public interface IProfilesContextFacade
{
    /// <summary>
    ///     Create a profile
    /// </summary>
    /// <param name="firstName">
    ///     First name of the profile
    /// </param>
    /// <param name="lastName">
    ///     Last name of the profile
    /// </param>
    /// <param name="email">
    ///     Email of the profile
    /// </param>
    /// <param name="role">
    ///     Role of the profile (Owner, Driver or Mechanic)
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     The id of the created profile if successful, 0 otherwise
    /// </returns>
    Task<int> CreateProfile(string firstName, string lastName, string email, string role,
        CancellationToken cancellationToken);

    /// <summary>
    ///     Fetch the profile id by email
    /// </summary>
    /// <param name="email">
    ///     Email of the profile to fetch
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     The id of the profile if found, 0 otherwise
    /// </returns>
    Task<int> FetchProfileIdByEmail(string email, CancellationToken cancellationToken);

    /// <summary>
    ///     Check if a profile exists by its id
    /// </summary>
    /// <param name="profileId">
    ///     The id of the profile
    /// </param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     True if the profile exists, false otherwise
    /// </returns>
    Task<bool> ExistsProfileById(int profileId, CancellationToken cancellationToken);
}

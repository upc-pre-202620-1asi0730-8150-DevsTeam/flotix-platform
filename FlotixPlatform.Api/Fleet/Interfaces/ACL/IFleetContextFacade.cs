namespace FlotixPlatform.Api.Fleet.Interfaces.ACL;

/// <summary>
///     Facade (anti-corruption layer) that exposes the Vehicle &amp; Fleet Management data other bounded contexts need.
/// </summary>
public interface IFleetContextFacade
{
    /// <summary>
    ///     Fetches the vehicles of an owner with their license plates.
    /// </summary>
    /// <param name="ownerId">The identifier of the owner user.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The license plate of each vehicle of the owner, by vehicle identifier.</returns>
    Task<IReadOnlyDictionary<Guid, string>> FetchLicensePlatesByOwnerIdAsync(Guid ownerId,
        CancellationToken cancellationToken = default);
}
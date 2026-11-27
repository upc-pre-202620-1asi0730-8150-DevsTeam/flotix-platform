using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Notifications.Domain.Repositories;

/// <summary>
///     The alert repository contract.
/// </summary>
public interface IAlertRepository : IBaseRepository<Alert>
{
    /// <summary>
    ///     Find alerts by vehicle ID
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the alert objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Alert>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
}

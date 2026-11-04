using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Incidents.Domain.Repositories;

/// <summary>
///     The incident repository contract.
/// </summary>
public interface IIncidentRepository : IBaseRepository<Incident>
{
    /// <summary>
    ///     Find incidents by vehicle ID
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the incident objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Incident>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find incidents by driver ID
    /// </summary>
    /// <param name="driverId">The driver ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the incident objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Incident>> FindByDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default);
}

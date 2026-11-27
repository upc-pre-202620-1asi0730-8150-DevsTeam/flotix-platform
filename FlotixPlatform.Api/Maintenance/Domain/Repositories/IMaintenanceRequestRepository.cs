using FlotixPlatform.Api.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Maintenance.Domain.Repositories;

/// <summary>
///     The maintenance request repository contract.
/// </summary>
public interface IMaintenanceRequestRepository : IBaseRepository<MaintenanceRequest>
{
    /// <summary>
    ///     Find maintenance requests by vehicle ID
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the maintenance request objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<MaintenanceRequest>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find maintenance requests by mechanic ID
    /// </summary>
    /// <param name="mechanicId">The mechanic ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the maintenance request objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<MaintenanceRequest>> FindByMechanicIdAsync(Guid mechanicId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find the maintenance requests of a set of vehicles requested within a period
    /// </summary>
    /// <param name="vehicleIds">The identifiers of the vehicles.</param>
    /// <param name="from">The start of the period (inclusive).</param>
    /// <param name="to">The end of the period (exclusive).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the maintenance request objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<MaintenanceRequest>> FindByVehicleIdsAndRequestDateRangeAsync(IReadOnlyCollection<Guid> vehicleIds,
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
}
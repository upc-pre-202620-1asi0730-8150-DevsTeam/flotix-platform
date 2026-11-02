using FlotixPlatform.Api.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Fuel.Domain.Repositories;

/// <summary>
///     The fuel record repository contract.
/// </summary>
public interface IFuelRecordRepository : IBaseRepository<FuelRecord>
{
    /// <summary>
    ///     Find fuel records by vehicle ID
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the fuel record objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<FuelRecord>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find fuel records by driver ID
    /// </summary>
    /// <param name="driverId">The driver ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the fuel record objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<FuelRecord>> FindByDriverIdAsync(Guid driverId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find the fuel records of a set of vehicles recorded within a period
    /// </summary>
    /// <param name="vehicleIds">The identifiers of the vehicles.</param>
    /// <param name="from">The start of the period (inclusive).</param>
    /// <param name="to">The end of the period (exclusive).</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the fuel record objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<FuelRecord>> FindByVehicleIdsAndRecordedDateRangeAsync(IReadOnlyCollection<Guid> vehicleIds,
        DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default);
}

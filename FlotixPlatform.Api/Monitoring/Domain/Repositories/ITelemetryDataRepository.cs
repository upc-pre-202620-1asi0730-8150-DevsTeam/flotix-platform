using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Monitoring.Domain.Repositories;

/// <summary>
///     The telemetry record repository contract.
/// </summary>
public interface ITelemetryDataRepository : IBaseRepository<TelemetryData>
{
    /// <summary>
    ///     Find telemetry records by device ID
    /// </summary>
    /// <param name="deviceId">The device ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the telemetry record objects from the most recent reading, or empty otherwise.
    /// </returns>
    Task<IEnumerable<TelemetryData>> FindByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken = default);
}

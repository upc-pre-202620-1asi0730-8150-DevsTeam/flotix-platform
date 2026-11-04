using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;

namespace FlotixPlatform.Api.Monitoring.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service contract to publish the updated location of a vehicle in real time.
/// </summary>
public interface IVehicleLocationNotifier
{
    /// <summary>
    ///     Publishes the location of a vehicle from its latest telemetry record (UpdateVehicleLocation).
    /// </summary>
    /// <param name="vehicleId">The identifier of the vehicle equipped with the reporting device.</param>
    /// <param name="telemetryData">The latest telemetry record of the vehicle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task NotifyAsync(Guid vehicleId, TelemetryData telemetryData, CancellationToken cancellationToken = default);
}

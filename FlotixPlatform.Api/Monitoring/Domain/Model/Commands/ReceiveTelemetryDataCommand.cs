namespace FlotixPlatform.Api.Monitoring.Domain.Model.Commands;

/// <summary>
///     Command to receive a telemetry packet sent by an IoT device
/// </summary>
/// <param name="DeviceId">The identifier of the reporting IoT device.</param>
/// <param name="Latitude">The latitude of the reported position.</param>
/// <param name="Longitude">The longitude of the reported position.</param>
/// <param name="Speed">The reported speed in km/h.</param>
/// <param name="RecordedAt">The moment the reading was taken by the device.</param>
public record ReceiveTelemetryDataCommand(Guid DeviceId, decimal Latitude, decimal Longitude, decimal Speed, DateTimeOffset RecordedAt);

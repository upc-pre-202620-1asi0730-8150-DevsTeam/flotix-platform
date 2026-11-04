namespace FlotixPlatform.Api.Monitoring.Domain.Model.Queries;

/// <summary>
///     Query to get all telemetry records by device ID
/// </summary>
/// <param name="DeviceId">The device ID to search</param>
public record GetAllTelemetryDataByDeviceIdQuery(Guid DeviceId);

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

public record TelemetryDataResource(
    Guid Id,
    Guid DeviceId,
    decimal Latitude,
    decimal Longitude,
    decimal Speed,
    DateTimeOffset RecordedAt);

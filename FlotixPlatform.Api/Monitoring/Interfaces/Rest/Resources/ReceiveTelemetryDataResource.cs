namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

public record ReceiveTelemetryDataResource(
    Guid? DeviceId,
    decimal? Latitude,
    decimal? Longitude,
    decimal? Speed,
    DateTimeOffset? RecordedAt);

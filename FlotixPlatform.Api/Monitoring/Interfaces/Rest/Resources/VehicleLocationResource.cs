namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

public record VehicleLocationResource(
    Guid VehicleId,
    Guid DeviceId,
    decimal Latitude,
    decimal Longitude,
    decimal Speed,
    DateTimeOffset RecordedAt);

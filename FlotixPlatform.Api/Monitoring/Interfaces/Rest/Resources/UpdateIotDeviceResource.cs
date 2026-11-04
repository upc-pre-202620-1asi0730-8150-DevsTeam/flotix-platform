namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

public record UpdateIotDeviceResource(
    Guid VehicleId,
    bool IsConnected);

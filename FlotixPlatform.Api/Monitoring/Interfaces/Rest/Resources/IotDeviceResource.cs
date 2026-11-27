namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;

public record IotDeviceResource(
    Guid Id,
    Guid VehicleId,
    string SerialNumber,
    bool IsConnected);

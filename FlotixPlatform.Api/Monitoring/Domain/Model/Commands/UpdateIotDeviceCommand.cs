namespace FlotixPlatform.Api.Monitoring.Domain.Model.Commands;

/// <summary>
///     Command to update an IoT device
/// </summary>
/// <param name="IotDeviceId">The identifier of the IoT device to update</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="IsConnected">Whether the IoT device is connected.</param>
public record UpdateIotDeviceCommand(Guid IotDeviceId, Guid VehicleId, bool IsConnected);

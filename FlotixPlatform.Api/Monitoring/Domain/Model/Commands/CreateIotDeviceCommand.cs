using FlotixPlatform.Api.Monitoring.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Monitoring.Domain.Model.Commands;

/// <summary>
///     Command to create an IoT device
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="SerialNumber">The serial number of the IoT device.</param>
/// <param name="IsConnected">Whether the IoT device is connected.</param>
public record CreateIotDeviceCommand(Guid VehicleId, SerialNumber SerialNumber, bool IsConnected);

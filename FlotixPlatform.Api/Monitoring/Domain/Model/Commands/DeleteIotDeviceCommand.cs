namespace FlotixPlatform.Api.Monitoring.Domain.Model.Commands;

/// <summary>
///     Command to delete an IoT device
/// </summary>
/// <param name="IotDeviceId">The identifier of the IoT device to delete</param>
public record DeleteIotDeviceCommand(Guid IotDeviceId);

using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Monitoring.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;

/// <summary>
///     Represents an IoT tracking device installed in a vehicle (at most one per vehicle).
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateIotDeviceCommand and updated through an UpdateIotDeviceCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class IotDevice
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected IotDevice()
    {
        SerialNumber = null!;
    }

    /// <summary>
    ///     Creates a new IotDevice from a creation command.
    /// </summary>
    /// <param name="command">The CreateIotDeviceCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public IotDevice(CreateIotDeviceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.VehicleId, Guid.Empty);
        ArgumentNullException.ThrowIfNull(command.SerialNumber);

        VehicleId = command.VehicleId;
        SerialNumber = command.SerialNumber;
        IsConnected = command.IsConnected;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this IoT device.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the vehicle.
    /// </summary>
    public Guid VehicleId { get; private set; }

    /// <summary>
    ///     Gets the serial number of the IoT device.
    /// </summary>
    public SerialNumber SerialNumber { get; private set; }

    /// <summary>
    ///     Gets whether the IoT device is connected.
    /// </summary>
    public bool IsConnected { get; private set; }

    /// <summary>
    ///     Marks the IoT device as connected, as it is sending telemetry to the platform.
    /// </summary>
    public void Connect()
    {
        IsConnected = true;
    }

    /// <summary>
    ///     Updates the IoT device information from an update command.
    /// </summary>
    /// <param name="command">The UpdateIotDeviceCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateIotDeviceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.VehicleId, Guid.Empty);

        VehicleId = command.VehicleId;
        IsConnected = command.IsConnected;
    }
}

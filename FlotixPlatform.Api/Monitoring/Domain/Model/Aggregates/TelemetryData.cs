using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;

namespace FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;

/// <summary>
///     Represents a position and speed reading reported by an IoT device; readings are immutable.
/// </summary>
/// <remarks>
///     This aggregate root is created by the IoT Telemetry Processor from a ReceiveTelemetryDataCommand
///     and is immutable afterwards.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class TelemetryData
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected TelemetryData()
    {
    }

    /// <summary>
    ///     Creates a new TelemetryData from a received telemetry packet.
    /// </summary>
    /// <param name="command">The ReceiveTelemetryDataCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the packet does not follow the expected schema.</exception>
    public TelemetryData(ReceiveTelemetryDataCommand command)
    {
        Validate(command);

        DeviceId = command.DeviceId;
        Latitude = command.Latitude;
        Longitude = command.Longitude;
        Speed = command.Speed;
        RecordedAt = command.RecordedAt;
    }

    /// <summary>
    ///     Validates that a received telemetry packet follows the expected schema.
    /// </summary>
    /// <remarks>
    ///     Used when the packet is received, so corrupt packets are discarded before being queued,
    ///     and again when the aggregate is created.
    /// </remarks>
    /// <param name="command">The ReceiveTelemetryDataCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the packet does not follow the expected schema.</exception>
    public static void Validate(ReceiveTelemetryDataCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.DeviceId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.Latitude, -90m);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Latitude, 90m);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.Longitude, -180m);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Longitude, 180m);
        ArgumentOutOfRangeException.ThrowIfNegative(command.Speed);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Speed, 400m);
        ArgumentOutOfRangeException.ThrowIfEqual(command.RecordedAt, default);
    }

    /// <summary>
    ///     Gets the server-generated identifier for this telemetry record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the reporting IoT device.
    /// </summary>
    public Guid DeviceId { get; private set; }

    /// <summary>
    ///     Gets the latitude of the reported position.
    /// </summary>
    public decimal Latitude { get; private set; }

    /// <summary>
    ///     Gets the longitude of the reported position.
    /// </summary>
    public decimal Longitude { get; private set; }

    /// <summary>
    ///     Gets the reported speed in km/h.
    /// </summary>
    public decimal Speed { get; private set; }

    /// <summary>
    ///     Gets the moment the reading was taken by the device.
    /// </summary>
    public DateTimeOffset RecordedAt { get; private set; }
}

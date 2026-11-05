using FlotixPlatform.Api.Notifications.Domain.Model.Commands;

namespace FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;

/// <summary>
///     Represents an alert raised for a vehicle (e.g. speeding or upcoming maintenance).
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateAlertCommand and is immutable afterwards.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class Alert
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected Alert()
    {
        AlertType = string.Empty;
    }

    /// <summary>
    ///     Creates a new Alert from a creation command.
    /// </summary>
    /// <param name="command">The CreateAlertCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public Alert(CreateAlertCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.VehicleId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.AlertType);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.AlertType.Length, 50);

        VehicleId = command.VehicleId;
        AlertType = command.AlertType;
        TriggerDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this alert.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the vehicle.
    /// </summary>
    public Guid VehicleId { get; private set; }

    /// <summary>
    ///     Gets the type of the alert (e.g. Exceso de velocidad).
    /// </summary>
    public string AlertType { get; private set; }

    /// <summary>
    ///     Gets the moment the alert was triggered.
    /// </summary>
    public DateTimeOffset TriggerDate { get; private set; }
}

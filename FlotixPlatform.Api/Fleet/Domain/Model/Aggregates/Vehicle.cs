using FlotixPlatform.API.Fleet.Domain.Model.Commands;
using FlotixPlatform.API.Fleet.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Fleet.Domain.Model.Aggregates;

/// <summary>
///     Represents a vehicle of a fleet, owned by an owner user and optionally assigned to a driver.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateVehicleCommand and updated through an UpdateVehicleCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class Vehicle
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected Vehicle()
    {
        LicensePlate = null!;
        VehicleType = string.Empty;
    }

    /// <summary>
    ///     Creates a new Vehicle from a creation command.
    /// </summary>
    /// <param name="command">The CreateVehicleCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public Vehicle(CreateVehicleCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.OwnerId, Guid.Empty);
        ArgumentNullException.ThrowIfNull(command.LicensePlate);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.VehicleType);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.VehicleType.Length, 50);
        ArgumentOutOfRangeException.ThrowIfNegative(command.CurrentMileage);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.CurrentMileage, 10000000m);

        OwnerId = command.OwnerId;
        DriverId = command.DriverId;
        LicensePlate = command.LicensePlate;
        VehicleType = command.VehicleType;
        CurrentMileage = command.CurrentMileage;
        Status = command.Status;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this vehicle.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the owner user.
    /// </summary>
    public Guid OwnerId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the driver user.
    /// </summary>
    public Guid? DriverId { get; private set; }

    /// <summary>
    ///     Gets the license plate of the vehicle.
    /// </summary>
    public LicensePlate LicensePlate { get; private set; }

    /// <summary>
    ///     Gets the type of the vehicle (e.g. Camión de carga).
    /// </summary>
    public string VehicleType { get; private set; }

    /// <summary>
    ///     Gets the current mileage of the vehicle in kilometers.
    /// </summary>
    public decimal CurrentMileage { get; private set; }

    /// <summary>
    ///     Gets the current status of the vehicle.
    /// </summary>
    public VehicleStatus Status { get; private set; }

    /// <summary>
    ///     Updates the vehicle information from an update command.
    /// </summary>
    /// <param name="command">The UpdateVehicleCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the update breaks the rule: The mileage of a vehicle cannot decrease.</exception>
    public void UpdateInformation(UpdateVehicleCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.CurrentMileage < CurrentMileage)
            throw new InvalidOperationException("The mileage of a vehicle cannot decrease.");

        ArgumentNullException.ThrowIfNull(command.LicensePlate);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.VehicleType);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.VehicleType.Length, 50);
        ArgumentOutOfRangeException.ThrowIfNegative(command.CurrentMileage);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.CurrentMileage, 10000000m);

        DriverId = command.DriverId;
        LicensePlate = command.LicensePlate;
        VehicleType = command.VehicleType;
        CurrentMileage = command.CurrentMileage;
        Status = command.Status;
    }
}
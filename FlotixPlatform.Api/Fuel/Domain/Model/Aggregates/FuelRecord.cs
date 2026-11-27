using FlotixPlatform.Api.Fuel.Domain.Model.Commands;

namespace FlotixPlatform.Api.Fuel.Domain.Model.Aggregates;

/// <summary>
///     Represents a refuel of a vehicle registered by a driver.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateFuelRecordCommand and updated through an UpdateFuelRecordCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class FuelRecord
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected FuelRecord()
    {
    }

    /// <summary>
    ///     Creates a new FuelRecord from a creation command.
    /// </summary>
    /// <param name="command">The CreateFuelRecordCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public FuelRecord(CreateFuelRecordCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.VehicleId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfEqual(command.DriverId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.Liters);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Liters, 10000m);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.Cost);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Cost, 1000000m);
        ArgumentOutOfRangeException.ThrowIfNegative(command.MileageAtRefill);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.MileageAtRefill, 10000000m);

        VehicleId = command.VehicleId;
        DriverId = command.DriverId;
        Liters = command.Liters;
        Cost = command.Cost;
        MileageAtRefill = command.MileageAtRefill;
        RecordedDate = command.RecordedDate;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this fuel record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the vehicle.
    /// </summary>
    public Guid VehicleId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the driver user.
    /// </summary>
    public Guid DriverId { get; private set; }

    /// <summary>
    ///     Gets the refueled amount in liters.
    /// </summary>
    public decimal Liters { get; private set; }

    /// <summary>
    ///     Gets the total cost of the refuel.
    /// </summary>
    public decimal Cost { get; private set; }

    /// <summary>
    ///     Gets the vehicle mileage when refueling.
    /// </summary>
    public decimal MileageAtRefill { get; private set; }

    /// <summary>
    ///     Gets the date and time of the refuel.
    /// </summary>
    public DateTimeOffset RecordedDate { get; private set; }

    /// <summary>
    ///     Updates the fuel record information from an update command.
    /// </summary>
    /// <param name="command">The UpdateFuelRecordCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateFuelRecordCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.Liters);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Liters, 10000m);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.Cost);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Cost, 1000000m);
        ArgumentOutOfRangeException.ThrowIfNegative(command.MileageAtRefill);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.MileageAtRefill, 10000000m);

        Liters = command.Liters;
        Cost = command.Cost;
        MileageAtRefill = command.MileageAtRefill;
        RecordedDate = command.RecordedDate;
    }
}

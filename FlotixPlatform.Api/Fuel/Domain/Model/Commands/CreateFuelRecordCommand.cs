namespace FlotixPlatform.Api.Fuel.Domain.Model.Commands;

/// <summary>
///     Command to create a fuel record
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="Cost">The total cost of the refuel.</param>
/// <param name="MileageAtRefill">The vehicle mileage when refueling.</param>
/// <param name="RecordedDate">The date and time of the refuel.</param>
public record CreateFuelRecordCommand(Guid VehicleId, Guid DriverId, decimal Liters, decimal Cost, decimal MileageAtRefill, DateTimeOffset RecordedDate);

namespace FlotixPlatform.Api.Fuel.Domain.Model.Commands;

/// <summary>
///     Command to update a fuel record
/// </summary>
/// <param name="FuelRecordId">The identifier of the fuel record to update</param>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="Cost">The total cost of the refuel.</param>
/// <param name="MileageAtRefill">The vehicle mileage when refueling.</param>
/// <param name="RecordedDate">The date and time of the refuel.</param>
public record UpdateFuelRecordCommand(Guid FuelRecordId, decimal Liters, decimal Cost, decimal MileageAtRefill, DateTimeOffset RecordedDate);

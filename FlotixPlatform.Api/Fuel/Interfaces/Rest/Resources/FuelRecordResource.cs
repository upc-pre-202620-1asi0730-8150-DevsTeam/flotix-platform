namespace FlotixPlatform.Api.Fuel.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a fuel record.
/// </summary>
/// <param name="Id">The server-generated ID of the fuel record</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="Cost">The total cost of the refuel.</param>
/// <param name="MileageAtRefill">The vehicle mileage when refueling.</param>
/// <param name="RecordedDate">The date and time of the refuel.</param>
public record FuelRecordResource(
    Guid Id,
    Guid VehicleId,
    Guid DriverId,
    decimal Liters,
    decimal Cost,
    decimal MileageAtRefill,
    DateTimeOffset RecordedDate);

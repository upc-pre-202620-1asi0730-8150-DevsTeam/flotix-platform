using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.Api.Fuel.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a fuel record.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="Cost">The total cost of the refuel.</param>
/// <param name="MileageAtRefill">The vehicle mileage when refueling.</param>
/// <param name="RecordedDate">The date and time of the refuel.</param>
public record CreateFuelRecordResource(
    Guid VehicleId,
    Guid DriverId,
    [Range(0.01, 10000, ErrorMessage = "RangeField")]
    decimal Liters,
    [Range(0.01, 1000000, ErrorMessage = "RangeField")]
    decimal Cost,
    [Range(0, 10000000, ErrorMessage = "RangeField")]
    decimal MileageAtRefill,
    DateTimeOffset RecordedDate);

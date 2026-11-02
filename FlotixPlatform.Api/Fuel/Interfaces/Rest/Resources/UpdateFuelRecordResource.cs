using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.Api.Fuel.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update a fuel record.
/// </summary>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="Cost">The total cost of the refuel.</param>
/// <param name="MileageAtRefill">The vehicle mileage when refueling.</param>
/// <param name="RecordedDate">The date and time of the refuel.</param>
public record UpdateFuelRecordResource(
    [Range(0.01, 10000, ErrorMessage = "RangeField")]
    decimal Liters,
    [Range(0.01, 1000000, ErrorMessage = "RangeField")]
    decimal Cost,
    [Range(0, 10000000, ErrorMessage = "RangeField")]
    decimal MileageAtRefill,
    DateTimeOffset RecordedDate);

using System.ComponentModel.DataAnnotations;
using FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update a vehicle.
/// </summary>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="LicensePlate">The license plate of the vehicle. (required)</param>
/// <param name="VehicleType">The type of the vehicle (e.g. Camión de carga). (required)</param>
/// <param name="CurrentMileage">The current mileage of the vehicle in kilometers.</param>
/// <param name="Status">The current status of the vehicle.</param>
public record UpdateVehicleResource(
    Guid? DriverId,
    [Required(ErrorMessage = "RequiredLicensePlate")]
    [StringLength(15, ErrorMessage = "StringLengthField")]
    string LicensePlate,
    [Required(ErrorMessage = "RequiredVehicleType")]
    [StringLength(50, ErrorMessage = "StringLengthField")]
    string VehicleType,
    [Range(0, 10000000, ErrorMessage = "RangeField")]
    decimal CurrentMileage,
    VehicleStatus Status);
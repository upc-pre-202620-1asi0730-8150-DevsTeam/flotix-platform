namespace FlotixPlatform.Api.Fleet.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating a vehicle.
/// </summary>
public enum UpdateVehicleError
{
    /// <summary>
    /// The vehicle to update does not exist.
    /// </summary>
    VehicleNotFound,

    /// <summary>
    /// The provided vehicle data is invalid.
    /// </summary>
    InvalidVehicle,

    /// <summary>
    /// A vehicle with the same license plate already exists.
    /// </summary>
    DuplicateVehicleLicensePlate,

    /// <summary>
    /// A resource referenced by the command does not exist.
    /// </summary>
    ReferencedResourceNotFound,

    /// <summary>
    /// The mileage of a vehicle cannot decrease.
    /// </summary>
    MileageCannotDecrease,

    /// <summary>
    /// An unexpected error occurred during the operation.
    /// </summary>
    UnexpectedError
}
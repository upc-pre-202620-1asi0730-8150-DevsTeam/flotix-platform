namespace FlotixPlatform.Api.Fleet.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a vehicle.
/// </summary>
public enum CreateVehicleError
{
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
    /// An unexpected error occurred during the operation.
    /// </summary>
    UnexpectedError
}
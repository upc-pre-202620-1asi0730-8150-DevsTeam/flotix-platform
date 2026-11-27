namespace FlotixPlatform.Api.Fleet.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a vehicle.
/// </summary>
public enum DeleteVehicleError
{
    /// <summary>
    /// The vehicle to delete does not exist.
    /// </summary>
    VehicleNotFound,

    /// <summary>
    /// The vehicle is referenced by other records and cannot be deleted.
    /// </summary>
    VehicleHasDependents,

    /// <summary>
    /// An unexpected error occurred during the operation.
    /// </summary>
    UnexpectedError
}
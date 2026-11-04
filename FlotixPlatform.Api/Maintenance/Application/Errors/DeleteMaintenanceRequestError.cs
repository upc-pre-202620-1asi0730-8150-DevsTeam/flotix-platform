namespace FlotixPlatform.API.Maintenance.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a maintenance request.
/// </summary>
public enum DeleteMaintenanceRequestError
{
	/// <summary>
	/// The maintenance request to delete does not exist.
	/// </summary>
	MaintenanceRequestNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

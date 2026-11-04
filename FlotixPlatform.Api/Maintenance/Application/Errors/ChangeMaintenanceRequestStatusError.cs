namespace FlotixPlatform.API.Maintenance.Application.Errors;

/// <summary>
/// Represents errors that can occur when moving a maintenance request through its lifecycle
/// (diagnostic and quote, budget decision, repair start and completion).
/// </summary>
public enum ChangeMaintenanceRequestStatusError
{
	/// <summary>
	/// The maintenance request does not exist.
	/// </summary>
	MaintenanceRequestNotFound,

	/// <summary>
	/// The provided data is invalid (e.g. a negative quote).
	/// </summary>
	InvalidMaintenanceRequest,

	/// <summary>
	/// The request is not in the status the transition requires, or it is already closed.
	/// </summary>
	InvalidStatusTransition,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

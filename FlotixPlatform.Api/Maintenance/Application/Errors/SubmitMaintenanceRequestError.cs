namespace FlotixPlatform.API.Maintenance.Application.Errors;

/// <summary>
/// Represents errors that can occur when submitting a maintenance request.
/// </summary>
public enum SubmitMaintenanceRequestError
{
	/// <summary>
	/// The provided maintenance request data is invalid.
	/// </summary>
	InvalidMaintenanceRequest,

	/// <summary>
	/// The vehicle or the mechanic referenced by the request does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

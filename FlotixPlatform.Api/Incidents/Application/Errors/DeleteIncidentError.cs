namespace FlotixPlatform.Api.Incidents.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting an incident.
/// </summary>
public enum DeleteIncidentError
{
	/// <summary>
	/// The incident to delete does not exist.
	/// </summary>
	IncidentNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

namespace FlotixPlatform.Api.Incidents.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating an incident.
/// </summary>
public enum UpdateIncidentError
{
	/// <summary>
	/// The incident to update does not exist.
	/// </summary>
	IncidentNotFound,

	/// <summary>
	/// The provided incident data is invalid.
	/// </summary>
	InvalidIncident,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

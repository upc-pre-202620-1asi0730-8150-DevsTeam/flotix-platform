namespace FlotixPlatform.Api.Incidents.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating an incident.
/// </summary>
public enum CreateIncidentError
{
	/// <summary>
	/// The provided incident data is invalid.
	/// </summary>
	InvalidIncident,

	/// <summary>
	/// A resource referenced by the command does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

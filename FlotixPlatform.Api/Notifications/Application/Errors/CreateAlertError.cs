namespace FlotixPlatform.Api.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating an alert.
/// </summary>
public enum CreateAlertError
{
	/// <summary>
	/// The provided alert data is invalid.
	/// </summary>
	InvalidAlert,

	/// <summary>
	/// A resource referenced by the command does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

namespace FlotixPlatform.Api.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a notification.
/// </summary>
public enum CreateNotificationError
{
	/// <summary>
	/// The provided notification data is invalid.
	/// </summary>
	InvalidNotification,

	/// <summary>
	/// A resource referenced by the command does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

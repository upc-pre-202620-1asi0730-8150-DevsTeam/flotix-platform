namespace FlotixPlatform.Api.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating a notification.
/// </summary>
public enum UpdateNotificationError
{
	/// <summary>
	/// The notification to update does not exist.
	/// </summary>
	NotificationNotFound,

	/// <summary>
	/// The provided notification data is invalid.
	/// </summary>
	InvalidNotification,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

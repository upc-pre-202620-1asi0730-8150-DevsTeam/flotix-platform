namespace FlotixPlatform.Api.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a notification.
/// </summary>
public enum DeleteNotificationError
{
	/// <summary>
	/// The notification to delete does not exist.
	/// </summary>
	NotificationNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}

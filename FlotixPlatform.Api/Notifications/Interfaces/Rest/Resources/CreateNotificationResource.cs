using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a notification.
/// </summary>
/// <param name="UserId">The identifier of the recipient user.</param>
/// <param name="AlertId">The identifier of the related alert, if any.</param>
/// <param name="Message">The message of the notification. (required)</param>
public record CreateNotificationResource(
    Guid UserId,
    Guid? AlertId,
    [Required(ErrorMessage = "RequiredMessage")]
    [StringLength(500, ErrorMessage = "StringLengthField")]
    string Message);

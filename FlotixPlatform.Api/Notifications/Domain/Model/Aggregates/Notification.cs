using FlotixPlatform.Api.Notifications.Domain.Model.Commands;

namespace FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;

/// <summary>
///     Represents a message sent to a user, optionally about an alert.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateNotificationCommand and updated through an UpdateNotificationCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class Notification
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected Notification()
    {
        Message = string.Empty;
    }

    /// <summary>
    ///     Creates a new Notification from a creation command.
    /// </summary>
    /// <param name="command">The CreateNotificationCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public Notification(CreateNotificationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.UserId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Message);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Message.Length, 500);

        UserId = command.UserId;
        AlertId = command.AlertId;
        Message = command.Message;
        IsRead = false;
        SentDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this notification.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the recipient user.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the related alert, if any.
    /// </summary>
    public Guid? AlertId { get; private set; }

    /// <summary>
    ///     Gets the message of the notification.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    ///     Gets whether the notification has been read.
    /// </summary>
    public bool IsRead { get; private set; }

    /// <summary>
    ///     Gets the moment the notification was sent.
    /// </summary>
    public DateTimeOffset SentDate { get; private set; }

    /// <summary>
    ///     Updates the notification information from an update command.
    /// </summary>
    /// <param name="command">The UpdateNotificationCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateNotificationCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        IsRead = command.IsRead;
    }
}

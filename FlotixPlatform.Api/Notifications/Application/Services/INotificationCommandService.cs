using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Domain.Model.Commands;
using FlotixPlatform.Api.Notifications.Application.Errors;
using FlotixPlatform.Api.Shared.Application.Patterns;

namespace FlotixPlatform.Api.Notifications.Application.Services;

/// <summary>
///     Command service interface for notification operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change notifications.
/// </remarks>
public interface INotificationCommandService
{
    /// <summary>
    ///     Handle the create notification command.
    /// </summary>
    /// <param name="command">CreateNotificationCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created Notification on success, or the error reason on failure.
    /// </returns>
    Task<Result<Notification, CreateNotificationError>> Handle(CreateNotificationCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update notification command.
    /// </summary>
    /// <param name="command">UpdateNotificationCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated Notification on success, or the error reason on failure.
    /// </returns>
    Task<Result<Notification, UpdateNotificationError>> Handle(UpdateNotificationCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete notification command.
    /// </summary>
    /// <param name="command">DeleteNotificationCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted Notification on success, or the error reason on failure.
    /// </returns>
    Task<Result<Notification, DeleteNotificationError>> Handle(DeleteNotificationCommand command, CancellationToken cancellationToken = default);
}

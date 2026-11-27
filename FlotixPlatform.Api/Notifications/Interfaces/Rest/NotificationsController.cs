using System.Net.Mime;
using FlotixPlatform.Api.Notifications.Application.Services;
using FlotixPlatform.Api.Notifications.Domain.Model.Commands;
using FlotixPlatform.Api.Notifications.Domain.Model.Queries;
using FlotixPlatform.Api.Notifications.Application.Errors;
using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Interfaces.Hubs;
using FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest;

/// <summary>
///     Notifications' controller.
/// </summary>
/// <param name="notificationCommandService">The Notification Command Service</param>
/// <param name="notificationQueryService">The Notification Query Service</param>
/// <param name="notificationsHub">The SignalR hub used to push real-time events</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Notifications")]
public class NotificationsController(
    INotificationCommandService notificationCommandService,
    INotificationQueryService notificationQueryService,
    IHubContext<NotificationsHub, INotificationsClient> notificationsHub,
    IStringLocalizer<SharedResource> localizer,
    ILogger<NotificationsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a notification.
    /// </summary>
    /// <param name="resource">CreateNotificationResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created notification, or an error response if the notification was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateNotification([FromBody] CreateNotificationResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createNotificationCommand = CreateNotificationCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await notificationCommandService.Handle(createNotificationCommand, cancellationToken);
            if (result is Result<Notification, CreateNotificationError>.Success success)
                await notificationsHub.Clients.User(success.Value.UserId.ToString()).NotificationReceived(
                    NotificationResourceFromEntityAssembler.ToResourceFromEntity(success.Value));
            return ActionResultFromCreateNotificationResultAssembler.ToActionResultFromCreateNotificationResult(
                result,
                this,
                localizer,
                nameof(GetNotificationById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating notification");
            return BadRequest(localizer["InvalidNotificationRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating notification");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingNotification"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all notifications, optionally filtered by user ID.
    /// </summary>
    /// <param name="userId">The user ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching notifications (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotificationResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllNotifications([FromQuery] Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        var notifications = userId is not null
            ? await notificationQueryService.Handle(new GetAllNotificationsByUserIdQuery(userId.Value), cancellationToken)
            : await notificationQueryService.Handle(new GetAllNotificationsQuery(), cancellationToken);
        var resources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a notification by id.
    /// </summary>
    /// <param name="id">The Notification ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the notification, or not found if the notification was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetNotificationById(Guid id, CancellationToken cancellationToken = default)
    {
        var getNotificationByIdQuery = new GetNotificationByIdQuery(id);
        var result = await notificationQueryService.Handle(getNotificationByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = NotificationResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates a notification.
    /// </summary>
    /// <param name="id">The Notification ID</param>
    /// <param name="resource">UpdateNotificationResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated notification, or an error response if the notification was
    ///     not updated.
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(NotificationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateNotification(Guid id, [FromBody] UpdateNotificationResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateNotificationCommand = UpdateNotificationCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await notificationCommandService.Handle(updateNotificationCommand, cancellationToken);
            return ActionResultFromUpdateNotificationResultAssembler.ToActionResultFromUpdateNotificationResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating notification {NotificationId}", id);
            return BadRequest(localizer["InvalidNotificationRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating notification {NotificationId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingNotification"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes a notification.
    /// </summary>
    /// <param name="id">The Notification ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the notification was not found.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteNotification(Guid id, CancellationToken cancellationToken)
    {
        var deleteNotificationCommand = new DeleteNotificationCommand(id);
        var result = await notificationCommandService.Handle(deleteNotificationCommand, cancellationToken);
        return ActionResultFromDeleteNotificationResultAssembler.ToActionResultFromDeleteNotificationResult(
            result,
            this,
            localizer);
    }
}

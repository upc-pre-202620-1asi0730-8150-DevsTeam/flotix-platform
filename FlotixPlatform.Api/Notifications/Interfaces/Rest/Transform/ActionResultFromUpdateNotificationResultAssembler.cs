using FlotixPlatform.Api.Notifications.Application.Errors;
using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update a notification in the application layer.
/// </summary>
public static class ActionResultFromUpdateNotificationResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update notification operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update a notification.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateNotificationResult(
        Result<Notification, UpdateNotificationError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<Notification, UpdateNotificationError>.Success success =>
                controller.Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Notification, UpdateNotificationError>.Failure failure =>
                failure.Error switch
                {
                    UpdateNotificationError.NotificationNotFound =>
                        controller.NotFound(localizer["NotificationNotFound"].Value),
                    UpdateNotificationError.InvalidNotification =>
                        controller.BadRequest(localizer["InvalidNotificationRequest"].Value),
                    UpdateNotificationError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingNotification"].Value,
                            statusCode: 500),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: 500)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500)
        };
}

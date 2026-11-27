using FlotixPlatform.Api.Notifications.Application.Errors;
using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create a notification in the application layer.
/// </summary>
public static class ActionResultFromCreateNotificationResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create notification operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create a notification.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getNotificationByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateNotificationResult(
        Result<Notification, CreateNotificationError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getNotificationByIdActionName) =>
        result switch
        {
            Result<Notification, CreateNotificationError>.Success success =>
                controller.CreatedAtAction(getNotificationByIdActionName, new { id = success.Value.Id },
                    NotificationResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Notification, CreateNotificationError>.Failure failure =>
                failure.Error switch
                {
                    CreateNotificationError.InvalidNotification =>
                        controller.BadRequest(localizer["InvalidNotificationRequest"].Value),
                    CreateNotificationError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["NotificationReferencedResourceNotFound"].Value),
                    CreateNotificationError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingNotification"].Value,
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

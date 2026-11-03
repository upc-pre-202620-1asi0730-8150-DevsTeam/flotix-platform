using FlotixPlatform.Api.Notifications.Application.Errors;
using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create an alert in the application layer.
/// </summary>
public static class ActionResultFromCreateAlertResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create alert operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create an alert.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getAlertByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateAlertResult(
        Result<Alert, CreateAlertError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getAlertByIdActionName) =>
        result switch
        {
            Result<Alert, CreateAlertError>.Success success =>
                controller.CreatedAtAction(getAlertByIdActionName, new { id = success.Value.Id },
                    AlertResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Alert, CreateAlertError>.Failure failure =>
                failure.Error switch
                {
                    CreateAlertError.InvalidAlert =>
                        controller.BadRequest(localizer["InvalidAlertRequest"].Value),
                    CreateAlertError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["AlertReferencedResourceNotFound"].Value),
                    CreateAlertError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingAlert"].Value,
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

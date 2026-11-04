using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to submit a maintenance request in the application layer.
/// </summary>
public static class ActionResultFromSubmitMaintenanceRequestResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a submit maintenance request operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to submit a maintenance request.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getMaintenanceRequestByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromSubmitMaintenanceRequestResult(
        Result<MaintenanceRequest, SubmitMaintenanceRequestError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getMaintenanceRequestByIdActionName) =>
        result switch
        {
            Result<MaintenanceRequest, SubmitMaintenanceRequestError>.Success success =>
                controller.CreatedAtAction(getMaintenanceRequestByIdActionName, new { id = success.Value.Id },
                    MaintenanceRequestResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaintenanceRequest, SubmitMaintenanceRequestError>.Failure failure =>
                failure.Error switch
                {
                    SubmitMaintenanceRequestError.InvalidMaintenanceRequest =>
                        controller.BadRequest(localizer["InvalidMaintenanceRequestRequest"].Value),

                    SubmitMaintenanceRequestError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["MaintenanceRequestReferencedResourceNotFound"].Value),

                    SubmitMaintenanceRequestError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorSubmittingMaintenanceRequest"].Value,
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

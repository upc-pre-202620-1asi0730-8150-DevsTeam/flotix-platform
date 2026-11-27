using FlotixPlatform.Api.Incidents.Application.Errors;
using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to delete an incident in the application layer.
/// </summary>
public static class ActionResultFromDeleteIncidentResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a delete incident operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to delete an incident.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromDeleteIncidentResult(
        Result<Incident, DeleteIncidentError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<Incident, DeleteIncidentError>.Success success =>
                controller.NoContent(),

            Result<Incident, DeleteIncidentError>.Failure failure =>
                failure.Error switch
                {
                    DeleteIncidentError.IncidentNotFound =>
                        controller.NotFound(localizer["IncidentNotFound"].Value),
                    DeleteIncidentError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorDeletingIncident"].Value,
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

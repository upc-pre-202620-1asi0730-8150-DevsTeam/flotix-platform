using FlotixPlatform.Api.Incidents.Application.Errors;
using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update an incident in the application layer.
/// </summary>
public static class ActionResultFromUpdateIncidentResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update incident operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update an incident.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateIncidentResult(
        Result<Incident, UpdateIncidentError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<Incident, UpdateIncidentError>.Success success =>
                controller.Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Incident, UpdateIncidentError>.Failure failure =>
                failure.Error switch
                {
                    UpdateIncidentError.IncidentNotFound =>
                        controller.NotFound(localizer["IncidentNotFound"].Value),
                    UpdateIncidentError.InvalidIncident =>
                        controller.BadRequest(localizer["InvalidIncidentRequest"].Value),
                    UpdateIncidentError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingIncident"].Value,
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

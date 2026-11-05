using FlotixPlatform.Api.Incidents.Application.Errors;
using FlotixPlatform.Api.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create an incident in the application layer.
/// </summary>
public static class ActionResultFromCreateIncidentResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create incident operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create an incident.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getIncidentByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateIncidentResult(
        Result<Incident, CreateIncidentError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getIncidentByIdActionName) =>
        result switch
        {
            Result<Incident, CreateIncidentError>.Success success =>
                controller.CreatedAtAction(getIncidentByIdActionName, new { id = success.Value.Id },
                    IncidentResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Incident, CreateIncidentError>.Failure failure =>
                failure.Error switch
                {
                    CreateIncidentError.InvalidIncident =>
                        controller.BadRequest(localizer["InvalidIncidentRequest"].Value),
                    CreateIncidentError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["IncidentReferencedResourceNotFound"].Value),
                    CreateIncidentError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingIncident"].Value,
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

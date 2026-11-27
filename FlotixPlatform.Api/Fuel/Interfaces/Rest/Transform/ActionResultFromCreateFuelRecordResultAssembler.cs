using FlotixPlatform.Api.Fuel.Application.Errors;
using FlotixPlatform.Api.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Fuel.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create a fuel record in the application layer.
/// </summary>
public static class ActionResultFromCreateFuelRecordResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create fuel record operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create a fuel record.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getFuelRecordByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateFuelRecordResult(
        Result<FuelRecord, CreateFuelRecordError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getFuelRecordByIdActionName) =>
        result switch
        {
            Result<FuelRecord, CreateFuelRecordError>.Success success =>
                controller.CreatedAtAction(getFuelRecordByIdActionName, new { id = success.Value.Id },
                    FuelRecordResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<FuelRecord, CreateFuelRecordError>.Failure failure =>
                failure.Error switch
                {
                    CreateFuelRecordError.InvalidFuelRecord =>
                        controller.BadRequest(localizer["InvalidFuelRecordRequest"].Value),
                    CreateFuelRecordError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["FuelRecordReferencedResourceNotFound"].Value),
                    CreateFuelRecordError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingFuelRecord"].Value,
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

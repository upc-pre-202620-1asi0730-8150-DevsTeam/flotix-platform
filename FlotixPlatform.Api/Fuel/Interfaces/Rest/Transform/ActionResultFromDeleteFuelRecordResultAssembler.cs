using FlotixPlatform.Api.Fuel.Application.Errors;
using FlotixPlatform.Api.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Fuel.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to delete a fuel record in the application layer.
/// </summary>
public static class ActionResultFromDeleteFuelRecordResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a delete fuel record operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to delete a fuel record.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromDeleteFuelRecordResult(
        Result<FuelRecord, DeleteFuelRecordError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<FuelRecord, DeleteFuelRecordError>.Success success =>
                controller.NoContent(),

            Result<FuelRecord, DeleteFuelRecordError>.Failure failure =>
                failure.Error switch
                {
                    DeleteFuelRecordError.FuelRecordNotFound =>
                        controller.NotFound(localizer["FuelRecordNotFound"].Value),
                    DeleteFuelRecordError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorDeletingFuelRecord"].Value,
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

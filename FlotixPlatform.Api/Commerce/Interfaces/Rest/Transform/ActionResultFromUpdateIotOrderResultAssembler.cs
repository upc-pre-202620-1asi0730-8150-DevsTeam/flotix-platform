using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update an IoT order in the application layer.
/// </summary>
public static class ActionResultFromUpdateIotOrderResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update IoT order operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update an IoT order.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateIotOrderResult(
        Result<IotOrder, UpdateIotOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<IotOrder, UpdateIotOrderError>.Success success =>
                controller.Ok(IotOrderResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<IotOrder, UpdateIotOrderError>.Failure failure =>
                failure.Error switch
                {
                    UpdateIotOrderError.IotOrderNotFound =>
                        controller.NotFound(localizer["IotOrderNotFound"].Value),
                    UpdateIotOrderError.InvalidIotOrder =>
                        controller.BadRequest(localizer["InvalidIotOrderRequest"].Value),
                    UpdateIotOrderError.IotOrderAlreadyClosed =>
                        controller.Conflict(localizer["IotOrderAlreadyClosed"].Value),
                    UpdateIotOrderError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingIotOrder"].Value,
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

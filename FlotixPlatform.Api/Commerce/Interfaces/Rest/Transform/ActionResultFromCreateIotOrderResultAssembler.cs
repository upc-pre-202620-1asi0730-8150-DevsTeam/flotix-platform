using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create an IoT order in the application layer.
/// </summary>
public static class ActionResultFromCreateIotOrderResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create IoT order operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create an IoT order.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getIotOrderByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateIotOrderResult(
        Result<IotOrder, CreateIotOrderError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getIotOrderByIdActionName) =>
        result switch
        {
            Result<IotOrder, CreateIotOrderError>.Success success =>
                controller.CreatedAtAction(getIotOrderByIdActionName, new { id = success.Value.Id },
                    IotOrderResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<IotOrder, CreateIotOrderError>.Failure failure =>
                failure.Error switch
                {
                    CreateIotOrderError.InvalidIotOrder =>
                        controller.BadRequest(localizer["InvalidIotOrderRequest"].Value),
                    CreateIotOrderError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["IotOrderReferencedResourceNotFound"].Value),
                    CreateIotOrderError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingIotOrder"].Value,
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

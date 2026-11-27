using FlotixPlatform.Api.Fleet.Application.Errors;
using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create a vehicle in the application layer.
/// </summary>
public static class ActionResultFromCreateVehicleResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create vehicle operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create a vehicle.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getVehicleByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateVehicleResult(
        Result<Vehicle, CreateVehicleError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getVehicleByIdActionName) =>
        result switch
        {
            Result<Vehicle, CreateVehicleError>.Success success =>
                controller.CreatedAtAction(getVehicleByIdActionName, new { id = success.Value.Id },
                    VehicleResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Vehicle, CreateVehicleError>.Failure failure =>
                failure.Error switch
                {
                    CreateVehicleError.InvalidVehicle =>
                        controller.BadRequest(localizer["InvalidVehicleRequest"].Value),
                    CreateVehicleError.DuplicateVehicleLicensePlate =>
                        controller.Conflict(localizer["DuplicateVehicleLicensePlate"].Value),
                    CreateVehicleError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["VehicleReferencedResourceNotFound"].Value),
                    CreateVehicleError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingVehicle"].Value,
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
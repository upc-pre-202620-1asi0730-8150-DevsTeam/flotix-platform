using FlotixPlatform.Api.Fleet.Application.Errors;
using FlotixPlatform.Api.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update a vehicle in the application layer.
/// </summary>
public static class ActionResultFromUpdateVehicleResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update vehicle operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update a vehicle.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateVehicleResult(
        Result<Vehicle, UpdateVehicleError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<Vehicle, UpdateVehicleError>.Success success =>
                controller.Ok(VehicleResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Vehicle, UpdateVehicleError>.Failure failure =>
                failure.Error switch
                {
                    UpdateVehicleError.VehicleNotFound =>
                        controller.NotFound(localizer["VehicleNotFound"].Value),
                    UpdateVehicleError.InvalidVehicle =>
                        controller.BadRequest(localizer["InvalidVehicleRequest"].Value),
                    UpdateVehicleError.DuplicateVehicleLicensePlate =>
                        controller.Conflict(localizer["DuplicateVehicleLicensePlate"].Value),
                    UpdateVehicleError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["VehicleReferencedResourceNotFound"].Value),
                    UpdateVehicleError.MileageCannotDecrease =>
                        controller.BadRequest(localizer["MileageCannotDecrease"].Value),
                    UpdateVehicleError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingVehicle"].Value,
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
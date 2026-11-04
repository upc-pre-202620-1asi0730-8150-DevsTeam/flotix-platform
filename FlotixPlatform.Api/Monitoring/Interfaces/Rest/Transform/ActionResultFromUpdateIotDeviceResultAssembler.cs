using FlotixPlatform.Api.Monitoring.Application.Errors;
using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class ActionResultFromUpdateIotDeviceResultAssembler
{
    public static ActionResult ToActionResultFromUpdateIotDeviceResult(
        Result<IotDevice, UpdateIotDeviceError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<IotDevice, UpdateIotDeviceError>.Success success =>
                controller.Ok(IotDeviceResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<IotDevice, UpdateIotDeviceError>.Failure failure =>
                failure.Error switch
                {
                    UpdateIotDeviceError.IotDeviceNotFound =>
                        controller.NotFound(localizer["IotDeviceNotFound"].Value),
                    UpdateIotDeviceError.InvalidIotDevice =>
                        controller.BadRequest(localizer["InvalidIotDeviceRequest"].Value),
                    UpdateIotDeviceError.DuplicateIotDeviceVehicleId =>
                        controller.Conflict(localizer["DuplicateIotDeviceVehicleId"].Value),
                    UpdateIotDeviceError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["IotDeviceReferencedResourceNotFound"].Value),
                    UpdateIotDeviceError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingIotDevice"].Value,
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

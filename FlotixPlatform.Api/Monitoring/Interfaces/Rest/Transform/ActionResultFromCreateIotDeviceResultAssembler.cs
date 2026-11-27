using FlotixPlatform.Api.Monitoring.Application.Errors;
using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class ActionResultFromCreateIotDeviceResultAssembler
{
    public static ActionResult ToActionResultFromCreateIotDeviceResult(
        Result<IotDevice, CreateIotDeviceError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getIotDeviceByIdActionName) =>
        result switch
        {
            Result<IotDevice, CreateIotDeviceError>.Success success =>
                controller.CreatedAtAction(getIotDeviceByIdActionName, new { id = success.Value.Id },
                    IotDeviceResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<IotDevice, CreateIotDeviceError>.Failure failure =>
                failure.Error switch
                {
                    CreateIotDeviceError.InvalidIotDevice =>
                        controller.BadRequest(localizer["InvalidIotDeviceRequest"].Value),
                    CreateIotDeviceError.DuplicateIotDeviceVehicleId =>
                        controller.Conflict(localizer["DuplicateIotDeviceVehicleId"].Value),
                    CreateIotDeviceError.DuplicateIotDeviceSerialNumber =>
                        controller.Conflict(localizer["DuplicateIotDeviceSerialNumber"].Value),
                    CreateIotDeviceError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["IotDeviceReferencedResourceNotFound"].Value),
                    CreateIotDeviceError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingIotDevice"].Value,
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

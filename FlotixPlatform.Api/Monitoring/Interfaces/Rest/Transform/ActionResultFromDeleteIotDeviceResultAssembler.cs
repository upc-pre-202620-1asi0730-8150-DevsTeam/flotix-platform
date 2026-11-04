using FlotixPlatform.Api.Monitoring.Application.Errors;
using FlotixPlatform.Api.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class ActionResultFromDeleteIotDeviceResultAssembler
{
    public static ActionResult ToActionResultFromDeleteIotDeviceResult(
        Result<IotDevice, DeleteIotDeviceError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<IotDevice, DeleteIotDeviceError>.Success success =>
                controller.NoContent(),

            Result<IotDevice, DeleteIotDeviceError>.Failure failure =>
                failure.Error switch
                {
                    DeleteIotDeviceError.IotDeviceNotFound =>
                        controller.NotFound(localizer["IotDeviceNotFound"].Value),
                    DeleteIotDeviceError.IotDeviceHasDependents =>
                        controller.Conflict(localizer["IotDeviceHasDependents"].Value),
                    DeleteIotDeviceError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorDeletingIotDevice"].Value,
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

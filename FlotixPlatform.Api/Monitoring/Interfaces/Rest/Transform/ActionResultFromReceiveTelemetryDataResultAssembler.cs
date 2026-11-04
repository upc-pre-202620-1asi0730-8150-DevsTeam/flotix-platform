using FlotixPlatform.Api.Monitoring.Application.Errors;
using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;

public static class ActionResultFromReceiveTelemetryDataResultAssembler
{
    public static ActionResult ToActionResultFromReceiveTelemetryDataResult(
        Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>.Success =>
                controller.Accepted(),

            Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>.Failure failure =>
                failure.Error switch
                {
                    ReceiveTelemetryDataError.InvalidTelemetryData =>
                        controller.BadRequest(localizer["InvalidTelemetryDataRequest"].Value),

                    ReceiveTelemetryDataError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorReceivingTelemetryData"].Value,
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

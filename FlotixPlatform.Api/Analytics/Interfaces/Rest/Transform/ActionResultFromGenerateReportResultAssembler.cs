using FlotixPlatform.Api.Analytics.Application.Errors;
using FlotixPlatform.Api.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.Api.Analytics.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to generate a report in the application layer.
/// </summary>
public static class ActionResultFromGenerateReportResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a generate report operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to generate a report.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getReportByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromGenerateReportResult(
        Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getReportByIdActionName) =>
        result switch
        {
            Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError>.Success success =>
                controller.CreatedAtAction(getReportByIdActionName, new { id = success.Value.Report.Id },
                    GeneratedReportResourceFromResultAssembler.ToResourceFromResult(success.Value.Report,
                        success.Value.Lines, localizer["NoRecordsForReport"].Value)),

            Result<(Report Report, IReadOnlyList<ReportLine> Lines), GenerateReportError>.Failure failure =>
                failure.Error switch
                {
                    GenerateReportError.InvalidReport =>
                        controller.BadRequest(localizer["InvalidReportRequest"].Value),

                    GenerateReportError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["ReportReferencedResourceNotFound"].Value),

                    GenerateReportError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorGeneratingReport"].Value,
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

using System.Net.Mime;
using FlotixPlatform.Api.Analytics.Application.Services;
using FlotixPlatform.Api.Analytics.Domain.Model.Commands;
using FlotixPlatform.Api.Analytics.Domain.Model.Queries;
using FlotixPlatform.Api.Analytics.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Analytics.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Analytics.Interfaces.Rest;

/// <summary>
///     Reports' controller.
/// </summary>
/// <param name="reportCommandService">The Report Command Service</param>
/// <param name="reportQueryService">The Report Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Reports")]
[Authorize(Role.Owner)]
public class ReportsController(
    IReportCommandService reportCommandService,
    IReportQueryService reportQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<ReportsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Generates a fuel or maintenance report of the fleet of an owner within a date range.
    /// </summary>
    /// <remarks>
    ///     The report metadata is stored; when there are no records in the period the report is returned with an
    ///     informative message instead of an error.
    /// </remarks>
    /// <param name="resource">GenerateReportResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the generated report, or an error response if it was not generated.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(GeneratedReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GenerateReport([FromBody] GenerateReportResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var generateReportCommand = GenerateReportCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await reportCommandService.Handle(generateReportCommand, cancellationToken);
            return ActionResultFromGenerateReportResultAssembler.ToActionResultFromGenerateReportResult(
                result,
                this,
                localizer,
                nameof(GetReportById));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while generating report");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorGeneratingReport"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all reports, optionally filtered by owner ID.
    /// </summary>
    /// <param name="ownerId">The owner ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching reports (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReportResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllReports([FromQuery] Guid? ownerId = null,
        CancellationToken cancellationToken = default)
    {
        var reports = ownerId is not null
            ? await reportQueryService.Handle(new GetAllReportsByOwnerIdQuery(ownerId.Value), cancellationToken)
            : await reportQueryService.Handle(new GetAllReportsQuery(), cancellationToken);
        var resources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a report by id.
    /// </summary>
    /// <param name="id">The Report ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the report, or not found if the report was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetReportById(Guid id, CancellationToken cancellationToken = default)
    {
        var getReportByIdQuery = new GetReportByIdQuery(id);
        var result = await reportQueryService.Handle(getReportByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = ReportResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Deletes a report.
    /// </summary>
    /// <param name="id">The Report ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the report was not found.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteReport(Guid id, CancellationToken cancellationToken)
    {
        var deleteReportCommand = new DeleteReportCommand(id);
        var result = await reportCommandService.Handle(deleteReportCommand, cancellationToken);
        return ActionResultFromDeleteReportResultAssembler.ToActionResultFromDeleteReportResult(
            result,
            this,
            localizer);
    }
}

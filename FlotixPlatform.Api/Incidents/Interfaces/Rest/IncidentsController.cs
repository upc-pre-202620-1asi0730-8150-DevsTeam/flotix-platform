using System.Net.Mime;
using FlotixPlatform.Api.Incidents.Application.Services;
using FlotixPlatform.Api.Incidents.Domain.Model.Commands;
using FlotixPlatform.Api.Incidents.Domain.Model.Queries;
using FlotixPlatform.Api.Incidents.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Incidents.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Incidents.Interfaces.Rest;

/// <summary>
///     Incidents' controller.
/// </summary>
/// <param name="incidentCommandService">The Incident Command Service</param>
/// <param name="incidentQueryService">The Incident Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Incidents")]
public class IncidentsController(
    IIncidentCommandService incidentCommandService,
    IIncidentQueryService incidentQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<IncidentsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates an incident.
    /// </summary>
    /// <param name="resource">CreateIncidentResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created incident, or an error response if the incident was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateIncident([FromBody] CreateIncidentResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createIncidentCommand = CreateIncidentCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await incidentCommandService.Handle(createIncidentCommand, cancellationToken);
            return ActionResultFromCreateIncidentResultAssembler.ToActionResultFromCreateIncidentResult(
                result,
                this,
                localizer,
                nameof(GetIncidentById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating incident");
            return BadRequest(localizer["InvalidIncidentRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating incident");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingIncident"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all incidents, optionally filtered by vehicle ID or driver ID.
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to filter by (optional)</param>
    /// <param name="driverId">The driver ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching incidents (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IncidentResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllIncidents([FromQuery] Guid? vehicleId = null,
        [FromQuery] Guid? driverId = null,
        CancellationToken cancellationToken = default)
    {
        var incidents = vehicleId is not null
            ? await incidentQueryService.Handle(new GetAllIncidentsByVehicleIdQuery(vehicleId.Value), cancellationToken)
            : driverId is not null
            ? await incidentQueryService.Handle(new GetAllIncidentsByDriverIdQuery(driverId.Value), cancellationToken)
            : await incidentQueryService.Handle(new GetAllIncidentsQuery(), cancellationToken);
        var resources = incidents.Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets an incident by id.
    /// </summary>
    /// <param name="id">The Incident ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the incident, or not found if the incident was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetIncidentById(Guid id, CancellationToken cancellationToken = default)
    {
        var getIncidentByIdQuery = new GetIncidentByIdQuery(id);
        var result = await incidentQueryService.Handle(getIncidentByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = IncidentResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates an incident.
    /// </summary>
    /// <param name="id">The Incident ID</param>
    /// <param name="resource">UpdateIncidentResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated incident, or an error response if the incident was
    ///     not updated.
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IncidentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateIncident(Guid id, [FromBody] UpdateIncidentResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateIncidentCommand = UpdateIncidentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await incidentCommandService.Handle(updateIncidentCommand, cancellationToken);
            return ActionResultFromUpdateIncidentResultAssembler.ToActionResultFromUpdateIncidentResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating incident {IncidentId}", id);
            return BadRequest(localizer["InvalidIncidentRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating incident {IncidentId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingIncident"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes an incident.
    /// </summary>
    /// <param name="id">The Incident ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the incident was not found.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteIncident(Guid id, CancellationToken cancellationToken)
    {
        var deleteIncidentCommand = new DeleteIncidentCommand(id);
        var result = await incidentCommandService.Handle(deleteIncidentCommand, cancellationToken);
        return ActionResultFromDeleteIncidentResultAssembler.ToActionResultFromDeleteIncidentResult(
            result,
            this,
            localizer);
    }
}

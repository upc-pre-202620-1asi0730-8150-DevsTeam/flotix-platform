using System.Net.Mime;
using FlotixPlatform.Api.Monitoring.Application.Services;
using FlotixPlatform.Api.Monitoring.Domain.Model.Queries;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Monitoring.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Monitoring.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Telemetry Data")]
public class TelemetryDataController(
    ITelemetryDataCommandService telemetryDataCommandService,
    ITelemetryDataQueryService telemetryDataQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<TelemetryDataController> logger)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ReceiveTelemetryData([FromBody] ReceiveTelemetryDataResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var receiveTelemetryDataCommand = ReceiveTelemetryDataCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await telemetryDataCommandService.Handle(receiveTelemetryDataCommand, cancellationToken);
            return ActionResultFromReceiveTelemetryDataResultAssembler.ToActionResultFromReceiveTelemetryDataResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Corrupt telemetry packet discarded");
            return BadRequest(localizer["InvalidTelemetryDataRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while receiving telemetry packet");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorReceivingTelemetryData"].Value,
                statusCode: 500);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TelemetryDataResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllTelemetryData([FromQuery] Guid? deviceId = null,
        CancellationToken cancellationToken = default)
    {
        var telemetryData = deviceId is not null
            ? await telemetryDataQueryService.Handle(new GetAllTelemetryDataByDeviceIdQuery(deviceId.Value), cancellationToken)
            : await telemetryDataQueryService.Handle(new GetAllTelemetryDataQuery(), cancellationToken);
        var resources = telemetryData.Select(TelemetryDataResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TelemetryDataResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetTelemetryDataById(Guid id, CancellationToken cancellationToken = default)
    {
        var getTelemetryDataByIdQuery = new GetTelemetryDataByIdQuery(id);
        var result = await telemetryDataQueryService.Handle(getTelemetryDataByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = TelemetryDataResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
}

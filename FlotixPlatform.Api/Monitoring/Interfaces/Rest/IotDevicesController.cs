using System.Net.Mime;
using FlotixPlatform.Api.Monitoring.Application.Services;
using FlotixPlatform.Api.Monitoring.Domain.Model.Commands;
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
[Tags("IoT devices")]
public class IotDevicesController(
    IIotDeviceCommandService iotDeviceCommandService,
    IIotDeviceQueryService iotDeviceQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<IotDevicesController> logger)
    : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(IotDeviceResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateIotDevice([FromBody] CreateIotDeviceResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createIotDeviceCommand = CreateIotDeviceCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await iotDeviceCommandService.Handle(createIotDeviceCommand, cancellationToken);
            return ActionResultFromCreateIotDeviceResultAssembler.ToActionResultFromCreateIotDeviceResult(
                result,
                this,
                localizer,
                nameof(GetIotDeviceById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating IoT device");
            return BadRequest(localizer["InvalidIotDeviceRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating IoT device");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingIotDevice"].Value,
                statusCode: 500);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IotDeviceResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllIotDevices([FromQuery] Guid? vehicleId = null,
        CancellationToken cancellationToken = default)
    {
        var iotDevices = vehicleId is not null
            ? await iotDeviceQueryService.Handle(new GetAllIotDevicesByVehicleIdQuery(vehicleId.Value), cancellationToken)
            : await iotDeviceQueryService.Handle(new GetAllIotDevicesQuery(), cancellationToken);
        var resources = iotDevices.Select(IotDeviceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IotDeviceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetIotDeviceById(Guid id, CancellationToken cancellationToken = default)
    {
        var getIotDeviceByIdQuery = new GetIotDeviceByIdQuery(id);
        var result = await iotDeviceQueryService.Handle(getIotDeviceByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = IotDeviceResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IotDeviceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateIotDevice(Guid id, [FromBody] UpdateIotDeviceResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateIotDeviceCommand = UpdateIotDeviceCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await iotDeviceCommandService.Handle(updateIotDeviceCommand, cancellationToken);
            return ActionResultFromUpdateIotDeviceResultAssembler.ToActionResultFromUpdateIotDeviceResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating IoT device {IotDeviceId}", id);
            return BadRequest(localizer["InvalidIotDeviceRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating IoT device {IotDeviceId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingIotDevice"].Value,
                statusCode: 500);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteIotDevice(Guid id, CancellationToken cancellationToken)
    {
        var deleteIotDeviceCommand = new DeleteIotDeviceCommand(id);
        var result = await iotDeviceCommandService.Handle(deleteIotDeviceCommand, cancellationToken);
        return ActionResultFromDeleteIotDeviceResultAssembler.ToActionResultFromDeleteIotDeviceResult(
            result,
            this,
            localizer);
    }
}

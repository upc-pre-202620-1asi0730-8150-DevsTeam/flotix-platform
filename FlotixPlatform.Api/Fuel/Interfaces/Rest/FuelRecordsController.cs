using System.Net.Mime;
using FlotixPlatform.Api.Fuel.Application.Services;
using FlotixPlatform.Api.Fuel.Domain.Model.Commands;
using FlotixPlatform.Api.Fuel.Domain.Model.Queries;
using FlotixPlatform.Api.Fuel.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Fuel.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Fuel.Interfaces.Rest;

/// <summary>
///     Fuel records' controller.
/// </summary>
/// <param name="fuelRecordCommandService">The FuelRecord Command Service</param>
/// <param name="fuelRecordQueryService">The FuelRecord Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Fuel Records")]
public class FuelRecordsController(
    IFuelRecordCommandService fuelRecordCommandService,
    IFuelRecordQueryService fuelRecordQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<FuelRecordsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a fuel record.
    /// </summary>
    /// <param name="resource">CreateFuelRecordResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created fuel record, or an error response if the fuel record was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(FuelRecordResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateFuelRecord([FromBody] CreateFuelRecordResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createFuelRecordCommand = CreateFuelRecordCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await fuelRecordCommandService.Handle(createFuelRecordCommand, cancellationToken);
            return ActionResultFromCreateFuelRecordResultAssembler.ToActionResultFromCreateFuelRecordResult(
                result,
                this,
                localizer,
                nameof(GetFuelRecordById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating fuel record");
            return BadRequest(localizer["InvalidFuelRecordRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating fuel record");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingFuelRecord"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all fuel records, optionally filtered by vehicle ID or driver ID.
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to filter by (optional)</param>
    /// <param name="driverId">The driver ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching fuel records (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FuelRecordResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllFuelRecords([FromQuery] Guid? vehicleId = null,
        [FromQuery] Guid? driverId = null,
        CancellationToken cancellationToken = default)
    {
        var fuelRecords = vehicleId is not null
            ? await fuelRecordQueryService.Handle(new GetAllFuelRecordsByVehicleIdQuery(vehicleId.Value), cancellationToken)
            : driverId is not null
            ? await fuelRecordQueryService.Handle(new GetAllFuelRecordsByDriverIdQuery(driverId.Value), cancellationToken)
            : await fuelRecordQueryService.Handle(new GetAllFuelRecordsQuery(), cancellationToken);
        var resources = fuelRecords.Select(FuelRecordResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a fuel record by id.
    /// </summary>
    /// <param name="id">The FuelRecord ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the fuel record, or not found if the fuel record was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FuelRecordResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetFuelRecordById(Guid id, CancellationToken cancellationToken = default)
    {
        var getFuelRecordByIdQuery = new GetFuelRecordByIdQuery(id);
        var result = await fuelRecordQueryService.Handle(getFuelRecordByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = FuelRecordResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates a fuel record.
    /// </summary>
    /// <param name="id">The FuelRecord ID</param>
    /// <param name="resource">UpdateFuelRecordResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated fuel record, or an error response if the fuel record was
    ///     not updated.
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(FuelRecordResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateFuelRecord(Guid id, [FromBody] UpdateFuelRecordResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateFuelRecordCommand = UpdateFuelRecordCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await fuelRecordCommandService.Handle(updateFuelRecordCommand, cancellationToken);
            return ActionResultFromUpdateFuelRecordResultAssembler.ToActionResultFromUpdateFuelRecordResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating fuel record {FuelRecordId}", id);
            return BadRequest(localizer["InvalidFuelRecordRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating fuel record {FuelRecordId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingFuelRecord"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes a fuel record.
    /// </summary>
    /// <param name="id">The FuelRecord ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the fuel record was not found.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteFuelRecord(Guid id, CancellationToken cancellationToken)
    {
        var deleteFuelRecordCommand = new DeleteFuelRecordCommand(id);
        var result = await fuelRecordCommandService.Handle(deleteFuelRecordCommand, cancellationToken);
        return ActionResultFromDeleteFuelRecordResultAssembler.ToActionResultFromDeleteFuelRecordResult(
            result,
            this,
            localizer);
    }
}

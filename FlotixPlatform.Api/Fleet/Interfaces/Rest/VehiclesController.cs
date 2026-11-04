using System.Net.Mime;
using FlotixPlatform.Api.Fleet.Application.Services;
using FlotixPlatform.Api.Fleet.Domain.Model.Commands;
using FlotixPlatform.Api.Fleet.Domain.Model.Queries;
using FlotixPlatform.Api.Fleet.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Fleet.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Fleet.Interfaces.Rest;

/// <summary>
///     Vehicles' controller.
/// </summary>
/// <param name="vehicleCommandService">The Vehicle Command Service</param>
/// <param name="vehicleQueryService">The Vehicle Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Vehicles")]
public class VehiclesController(
    IVehicleCommandService vehicleCommandService,
    IVehicleQueryService vehicleQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<VehiclesController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a vehicle.
    /// </summary>
    /// <param name="resource">CreateVehicleResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created vehicle, or an error response if the vehicle was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateVehicle([FromBody] CreateVehicleResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createVehicleCommand = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await vehicleCommandService.Handle(createVehicleCommand, cancellationToken);
            return ActionResultFromCreateVehicleResultAssembler.ToActionResultFromCreateVehicleResult(
                result,
                this,
                localizer,
                nameof(GetVehicleById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating vehicle");
            return BadRequest(localizer["InvalidVehicleRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating vehicle");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingVehicle"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all vehicles, optionally filtered by owner ID or driver ID.
    /// </summary>
    /// <param name="ownerId">The owner ID to filter by (optional)</param>
    /// <param name="driverId">The driver ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching vehicles (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllVehicles([FromQuery] Guid? ownerId = null,
        [FromQuery] Guid? driverId = null,
        CancellationToken cancellationToken = default)
    {
        var vehicles = ownerId is not null
            ? await vehicleQueryService.Handle(new GetAllVehiclesByOwnerIdQuery(ownerId.Value), cancellationToken)
            : driverId is not null
            ? await vehicleQueryService.Handle(new GetAllVehiclesByDriverIdQuery(driverId.Value), cancellationToken)
            : await vehicleQueryService.Handle(new GetAllVehiclesQuery(), cancellationToken);
        var resources = vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a vehicle by id.
    /// </summary>
    /// <param name="id">The Vehicle ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the vehicle, or not found if the vehicle was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetVehicleById(Guid id, CancellationToken cancellationToken = default)
    {
        var getVehicleByIdQuery = new GetVehicleByIdQuery(id);
        var result = await vehicleQueryService.Handle(getVehicleByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates a vehicle.
    /// </summary>
    /// <param name="id">The Vehicle ID</param>
    /// <param name="resource">UpdateVehicleResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated vehicle, or an error response if the vehicle was
    ///     not updated.
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(VehicleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateVehicle(Guid id, [FromBody] UpdateVehicleResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateVehicleCommand = UpdateVehicleCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await vehicleCommandService.Handle(updateVehicleCommand, cancellationToken);
            return ActionResultFromUpdateVehicleResultAssembler.ToActionResultFromUpdateVehicleResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating vehicle {VehicleId}", id);
            return BadRequest(localizer["InvalidVehicleRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating vehicle {VehicleId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingVehicle"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes a vehicle.
    /// </summary>
    /// <param name="id">The Vehicle ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the vehicle was not found.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteVehicle(Guid id, CancellationToken cancellationToken)
    {
        var deleteVehicleCommand = new DeleteVehicleCommand(id);
        var result = await vehicleCommandService.Handle(deleteVehicleCommand, cancellationToken);
        return ActionResultFromDeleteVehicleResultAssembler.ToActionResultFromDeleteVehicleResult(
            result,
            this,
            localizer);
    }
}
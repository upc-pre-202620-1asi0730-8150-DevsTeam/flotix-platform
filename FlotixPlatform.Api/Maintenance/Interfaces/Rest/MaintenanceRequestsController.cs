using System.Net.Mime;
using FlotixPlatform.API.Maintenance.Application.Services;
using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Model.Queries;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest;

/// <summary>
///     Maintenance requests' controller: lifecycle of the maintenance requests between owners and workshops.
/// </summary>
/// <param name="maintenanceRequestCommandService">The MaintenanceRequest Command Service</param>
/// <param name="maintenanceRequestQueryService">The MaintenanceRequest Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Maintenance Requests")]
public class MaintenanceRequestsController(
    IMaintenanceRequestCommandService maintenanceRequestCommandService,
    IMaintenanceRequestQueryService maintenanceRequestQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MaintenanceRequestsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Submits a maintenance request to a mechanic; the request starts in "Pendiente" status.
    /// </summary>
    /// <param name="resource">SubmitMaintenanceRequestResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the submitted maintenance request, or an error response if it was
    ///     not submitted.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SubmitMaintenanceRequest([FromBody] SubmitMaintenanceRequestResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var submitMaintenanceRequestCommand = SubmitMaintenanceRequestCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await maintenanceRequestCommandService.Handle(submitMaintenanceRequestCommand, cancellationToken);
            return ActionResultFromSubmitMaintenanceRequestResultAssembler.ToActionResultFromSubmitMaintenanceRequestResult(
                result,
                this,
                localizer,
                nameof(GetMaintenanceRequestById));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while submitting maintenance request");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorSubmittingMaintenanceRequest"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all maintenance requests, optionally filtered by vehicle ID or mechanic ID.
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to filter by (optional)</param>
    /// <param name="mechanicId">The mechanic ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching maintenance requests (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceRequestResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllMaintenanceRequests([FromQuery] Guid? vehicleId = null,
        [FromQuery] Guid? mechanicId = null,
        CancellationToken cancellationToken = default)
    {
        var maintenanceRequests = vehicleId is not null
            ? await maintenanceRequestQueryService.Handle(new GetAllMaintenanceRequestsByVehicleIdQuery(vehicleId.Value), cancellationToken)
            : mechanicId is not null
            ? await maintenanceRequestQueryService.Handle(new GetAllMaintenanceRequestsByMechanicIdQuery(mechanicId.Value), cancellationToken)
            : await maintenanceRequestQueryService.Handle(new GetAllMaintenanceRequestsQuery(), cancellationToken);
        var resources = maintenanceRequests.Select(MaintenanceRequestResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a maintenance request by id.
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the maintenance request, or not found if the maintenance request was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetMaintenanceRequestById(Guid id, CancellationToken cancellationToken = default)
    {
        var getMaintenanceRequestByIdQuery = new GetMaintenanceRequestByIdQuery(id);
        var result = await maintenanceRequestQueryService.Handle(getMaintenanceRequestByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = MaintenanceRequestResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Registers the diagnostic and quote of the workshop: "Pendiente" → "Presupuestado".
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="resource">RegisterDiagnosticAndQuoteResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the quoted maintenance request, not found if it does not exist,
    ///     or conflict if the request is not in the status the transition requires.
    /// </returns>
    [HttpPost("{id:guid}/diagnostic-and-quote")]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RegisterDiagnosticAndQuote(Guid id, [FromBody] RegisterDiagnosticAndQuoteResource resource,
        CancellationToken cancellationToken)
    {
        var registerDiagnosticAndQuoteCommand = RegisterDiagnosticAndQuoteCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await maintenanceRequestCommandService.Handle(registerDiagnosticAndQuoteCommand, cancellationToken);
        return ActionResultFromChangeMaintenanceRequestStatusResultAssembler.ToActionResultFromChangeMaintenanceRequestStatusResult(
            result,
            this,
            localizer);
    }

    /// <summary>
    ///     Approves or rejects the quoted budget: "Presupuestado" → "Aprobado" or "Rechazado".
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="resource">ApproveOrRejectBudgetResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the approved or rejected maintenance request, not found if it does not exist,
    ///     or conflict if the request is not in the status the transition requires.
    /// </returns>
    [HttpPost("{id:guid}/budget-decision")]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ApproveOrRejectBudget(Guid id, [FromBody] ApproveOrRejectBudgetResource resource,
        CancellationToken cancellationToken)
    {
        var approveOrRejectBudgetCommand = ApproveOrRejectBudgetCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await maintenanceRequestCommandService.Handle(approveOrRejectBudgetCommand, cancellationToken);
        return ActionResultFromChangeMaintenanceRequestStatusResultAssembler.ToActionResultFromChangeMaintenanceRequestStatusResult(
            result,
            this,
            localizer);
    }

    /// <summary>
    ///     Registers the start of the approved repair: "Aprobado" → "En reparación".
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the in repair maintenance request, not found if it does not exist,
    ///     or conflict if the request is not in the status the transition requires.
    /// </returns>
    [HttpPost("{id:guid}/repair-start")]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateRepairStatus(Guid id,
        CancellationToken cancellationToken)
    {
        var updateRepairStatusCommand = new UpdateRepairStatusCommand(id);
        var result = await maintenanceRequestCommandService.Handle(updateRepairStatusCommand, cancellationToken);
        return ActionResultFromChangeMaintenanceRequestStatusResultAssembler.ToActionResultFromChangeMaintenanceRequestStatusResult(
            result,
            this,
            localizer);
    }

    /// <summary>
    ///     Registers the completion of the repair: "En reparación" → "Completado".
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the completed maintenance request, not found if it does not exist,
    ///     or conflict if the request is not in the status the transition requires.
    /// </returns>
    [HttpPost("{id:guid}/completion")]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CompleteMaintenance(Guid id,
        CancellationToken cancellationToken)
    {
        var completeMaintenanceCommand = new CompleteMaintenanceCommand(id);
        var result = await maintenanceRequestCommandService.Handle(completeMaintenanceCommand, cancellationToken);
        return ActionResultFromChangeMaintenanceRequestStatusResultAssembler.ToActionResultFromChangeMaintenanceRequestStatusResult(
            result,
            this,
            localizer);
    }

    /// <summary>
    ///     Deletes a maintenance request.
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the maintenance request was not found.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteMaintenanceRequest(Guid id, CancellationToken cancellationToken)
    {
        var deleteMaintenanceRequestCommand = new DeleteMaintenanceRequestCommand(id);
        var result = await maintenanceRequestCommandService.Handle(deleteMaintenanceRequestCommand, cancellationToken);
        return ActionResultFromDeleteMaintenanceRequestResultAssembler.ToActionResultFromDeleteMaintenanceRequestResult(
            result,
            this,
            localizer);
    }
}

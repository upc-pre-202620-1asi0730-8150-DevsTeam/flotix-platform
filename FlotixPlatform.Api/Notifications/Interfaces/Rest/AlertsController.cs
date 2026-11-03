using System.Net.Mime;
using FlotixPlatform.Api.Notifications.Application.Services;
using FlotixPlatform.Api.Notifications.Domain.Model.Queries;
using FlotixPlatform.Api.Notifications.Application.Errors;
using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Interfaces.Hubs;
using FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;
using FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform;
using FlotixPlatform.Api.Resources;
using FlotixPlatform.Api.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest;

/// <summary>
///     Alerts' controller.
/// </summary>
/// <param name="alertCommandService">The Alert Command Service</param>
/// <param name="alertQueryService">The Alert Query Service</param>
/// <param name="notificationsHub">The SignalR hub used to push real-time events</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Alerts")]
public class AlertsController(
    IAlertCommandService alertCommandService,
    IAlertQueryService alertQueryService,
    IHubContext<NotificationsHub, INotificationsClient> notificationsHub,
    IStringLocalizer<SharedResource> localizer,
    ILogger<AlertsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates an alert.
    /// </summary>
    /// <param name="resource">CreateAlertResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created alert, or an error response if the alert was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateAlert([FromBody] CreateAlertResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createAlertCommand = CreateAlertCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await alertCommandService.Handle(createAlertCommand, cancellationToken);
            if (result is Result<Alert, CreateAlertError>.Success success)
                await notificationsHub.Clients.All.AlertActivated(
                    AlertResourceFromEntityAssembler.ToResourceFromEntity(success.Value));
            return ActionResultFromCreateAlertResultAssembler.ToActionResultFromCreateAlertResult(
                result,
                this,
                localizer,
                nameof(GetAlertById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating alert");
            return BadRequest(localizer["InvalidAlertRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating alert");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingAlert"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all alerts, optionally filtered by vehicle ID.
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching alerts (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AlertResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllAlerts([FromQuery] Guid? vehicleId = null,
        CancellationToken cancellationToken = default)
    {
        var alerts = vehicleId is not null
            ? await alertQueryService.Handle(new GetAllAlertsByVehicleIdQuery(vehicleId.Value), cancellationToken)
            : await alertQueryService.Handle(new GetAllAlertsQuery(), cancellationToken);
        var resources = alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets an alert by id.
    /// </summary>
    /// <param name="id">The Alert ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the alert, or not found if the alert was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AlertResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAlertById(Guid id, CancellationToken cancellationToken = default)
    {
        var getAlertByIdQuery = new GetAlertByIdQuery(id);
        var result = await alertQueryService.Handle(getAlertByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = AlertResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
}

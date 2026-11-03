using System.Net.Mime;
using FlotixPlatform.API.Commerce.Application.Services;
using FlotixPlatform.API.Commerce.Domain.Model.Queries;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest;

/// <summary>
///     IoT orders' controller.
/// </summary>
/// <param name="iotOrderCommandService">The IotOrder Command Service</param>
/// <param name="iotOrderQueryService">The IotOrder Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("IoT Orders")]
[Authorize(Role.Owner)]
public class IotOrdersController(
    IIotOrderCommandService iotOrderCommandService,
    IIotOrderQueryService iotOrderQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<IotOrdersController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates an IoT order.
    /// </summary>
    /// <param name="resource">CreateIotOrderResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created IoT order, or an error response if the IoT order was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(IotOrderResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateIotOrder([FromBody] CreateIotOrderResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createIotOrderCommand = CreateIotOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await iotOrderCommandService.Handle(createIotOrderCommand, cancellationToken);
            return ActionResultFromCreateIotOrderResultAssembler.ToActionResultFromCreateIotOrderResult(
                result,
                this,
                localizer,
                nameof(GetIotOrderById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating IoT order");
            return BadRequest(localizer["InvalidIotOrderRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating IoT order");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingIotOrder"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all IoT orders, optionally filtered by owner ID.
    /// </summary>
    /// <param name="ownerId">The owner ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching IoT orders (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IotOrderResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllIotOrders([FromQuery] Guid? ownerId = null,
        CancellationToken cancellationToken = default)
    {
        var iotOrders = ownerId is not null
            ? await iotOrderQueryService.Handle(new GetAllIotOrdersByOwnerIdQuery(ownerId.Value), cancellationToken)
            : await iotOrderQueryService.Handle(new GetAllIotOrdersQuery(), cancellationToken);
        var resources = iotOrders.Select(IotOrderResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets an IoT order by id.
    /// </summary>
    /// <param name="id">The IotOrder ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the IoT order, or not found if the IoT order was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IotOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetIotOrderById(Guid id, CancellationToken cancellationToken = default)
    {
        var getIotOrderByIdQuery = new GetIotOrderByIdQuery(id);
        var result = await iotOrderQueryService.Handle(getIotOrderByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = IotOrderResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates an IoT order.
    /// </summary>
    /// <param name="id">The IotOrder ID</param>
    /// <param name="resource">UpdateIotOrderResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated IoT order, or an error response if the IoT order was
    ///     not updated.
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IotOrderResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateIotOrder(Guid id, [FromBody] UpdateIotOrderResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateIotOrderCommand = UpdateIotOrderCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await iotOrderCommandService.Handle(updateIotOrderCommand, cancellationToken);
            return ActionResultFromUpdateIotOrderResultAssembler.ToActionResultFromUpdateIotOrderResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating IoT order {IotOrderId}", id);
            return BadRequest(localizer["InvalidIotOrderRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating IoT order {IotOrderId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingIotOrder"].Value,
                statusCode: 500);
        }
    }
}

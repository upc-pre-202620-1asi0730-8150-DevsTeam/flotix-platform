using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Commerce.Domain.Model.Commands;

/// <summary>
///     Command to update an IoT order
/// </summary>
/// <param name="IotOrderId">The identifier of the IoT order to update</param>
/// <param name="Status">The current status of the IoT order.</param>
public record UpdateIotOrderCommand(Guid IotOrderId, OrderStatus Status);

using FlotixPlatform.Api.Commerce.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Commerce.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update an IoT order.
/// </summary>
/// <param name="Status">The current status of the IoT order.</param>
public record UpdateIotOrderResource(
    OrderStatus Status);

using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an IoT order.
/// </summary>
/// <param name="Id">The server-generated ID of the IoT order</param>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="Quantity">The number of devices ordered.</param>
/// <param name="TotalAmount">The total amount, priced from the catalog.</param>
/// <param name="Status">The current status of the IoT order.</param>
/// <param name="OrderDate">The date the order was placed.</param>
public record IotOrderResource(
    Guid Id,
    Guid OwnerId,
    int Quantity,
    decimal TotalAmount,
    OrderStatus Status,
    DateTimeOffset OrderDate);

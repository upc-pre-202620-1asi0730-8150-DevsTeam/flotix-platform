using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create an IoT order.
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="Quantity">The number of devices ordered.</param>
/// <param name="TotalAmount">The total amount, priced from the catalog.</param>
public record CreateIotOrderResource(
    Guid OwnerId,
    [Range(1, 1000, ErrorMessage = "RangeField")]
    int Quantity,
    [Range(0, 1000000, ErrorMessage = "RangeField")]
    decimal TotalAmount);

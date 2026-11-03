namespace FlotixPlatform.API.Commerce.Domain.Model.Commands;

/// <summary>
///     Command to create an IoT order
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="Quantity">The number of devices ordered.</param>
/// <param name="TotalAmount">The total amount, priced from the catalog.</param>
public record CreateIotOrderCommand(Guid OwnerId, int Quantity, decimal TotalAmount);

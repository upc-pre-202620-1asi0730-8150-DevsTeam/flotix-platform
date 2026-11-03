using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a IotOrderResource from a IotOrder.
/// </summary>
public static class IotOrderResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a IotOrderResource from a IotOrder.
    /// </summary>
    /// <param name="entity">The IotOrder entity</param>
    /// <returns>
    ///     A IotOrderResource assembled from the IotOrder
    /// </returns>
    public static IotOrderResource ToResourceFromEntity(IotOrder entity) =>
        new(entity.Id, entity.OwnerId, entity.Quantity, entity.TotalAmount, entity.Status, entity.OrderDate);
}

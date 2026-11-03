using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateIotOrderCommand from an UpdateIotOrderResource.
/// </summary>
public static class UpdateIotOrderCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateIotOrderCommand from an UpdateIotOrderResource.
    /// </summary>
    /// <param name="iotOrderId">The identifier of the IoT order to update</param>
    /// <param name="resource">The UpdateIotOrderResource resource</param>
    /// <returns>
    ///     An UpdateIotOrderCommand assembled from the UpdateIotOrderResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateIotOrderCommand ToCommandFromResource(Guid iotOrderId, UpdateIotOrderResource resource) =>
        new(iotOrderId, resource.Status);
}

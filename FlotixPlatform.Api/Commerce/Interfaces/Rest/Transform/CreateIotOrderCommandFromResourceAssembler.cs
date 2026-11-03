using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateIotOrderCommand from a CreateIotOrderResource.
/// </summary>
public static class CreateIotOrderCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateIotOrderCommand from a CreateIotOrderResource.
    /// </summary>
    /// <param name="resource">The CreateIotOrderResource resource</param>
    /// <returns>
    ///     A CreateIotOrderCommand assembled from the CreateIotOrderResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateIotOrderCommand ToCommandFromResource(CreateIotOrderResource resource) =>
        new(resource.OwnerId, resource.Quantity, resource.TotalAmount);
}

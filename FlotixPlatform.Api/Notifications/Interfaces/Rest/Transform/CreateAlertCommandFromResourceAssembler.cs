using FlotixPlatform.Api.Notifications.Domain.Model.Commands;
using FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateAlertCommand from a CreateAlertResource.
/// </summary>
public static class CreateAlertCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateAlertCommand from a CreateAlertResource.
    /// </summary>
    /// <param name="resource">The CreateAlertResource resource</param>
    /// <returns>
    ///     A CreateAlertCommand assembled from the CreateAlertResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateAlertCommand ToCommandFromResource(CreateAlertResource resource) =>
        new(resource.VehicleId, resource.AlertType);
}

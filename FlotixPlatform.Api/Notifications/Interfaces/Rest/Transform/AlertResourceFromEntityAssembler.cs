using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a AlertResource from a Alert.
/// </summary>
public static class AlertResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a AlertResource from a Alert.
    /// </summary>
    /// <param name="entity">The Alert entity</param>
    /// <returns>
    ///     A AlertResource assembled from the Alert
    /// </returns>
    public static AlertResource ToResourceFromEntity(Alert entity) =>
        new(entity.Id, entity.VehicleId, entity.AlertType, entity.TriggerDate);
}

using FlotixPlatform.Api.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.Api.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.Api.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a NotificationResource from a Notification.
/// </summary>
public static class NotificationResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a NotificationResource from a Notification.
    /// </summary>
    /// <param name="entity">The Notification entity</param>
    /// <returns>
    ///     A NotificationResource assembled from the Notification
    /// </returns>
    public static NotificationResource ToResourceFromEntity(Notification entity) =>
        new(entity.Id, entity.UserId, entity.AlertId, entity.Message, entity.IsRead, entity.SentDate);
}

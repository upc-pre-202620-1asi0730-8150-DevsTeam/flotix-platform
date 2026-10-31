namespace FlotixPlatform.Api.Shared.Domain.Model;

/// <summary>
///     Defines an entity whose creation and last update timestamps are tracked.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    ///     Gets or sets the date and time when the entity was created.
    /// </summary>
    DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the entity was last updated.
    /// </summary>
    DateTimeOffset? UpdatedAt { get; set; }
}
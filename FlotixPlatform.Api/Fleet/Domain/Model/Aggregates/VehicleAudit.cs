using FlotixPlatform.API.Shared.Domain.Model;

namespace FlotixPlatform.API.Fleet.Domain.Model.Aggregates;

/// <summary>
///     Audit extension for Vehicle aggregate.
/// </summary>
/// <remarks>
///     This partial class extends Vehicle with audit trail properties.
///     CreatedAt and UpdatedAt are automatically managed by the persistence layer
///     via the AuditableEntityInterceptor in the Shared bounded context.
/// </remarks>
public partial class Vehicle : IAuditableEntity
{
    /// <summary>
    ///     Gets the timestamp when this vehicle was created.
    /// </summary>
    /// <remarks>
    ///     Automatically set once by the persistence layer on first insert. Maps to column: created_at.
    /// </remarks>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets the timestamp when this vehicle was last updated.
    /// </summary>
    /// <remarks>
    ///     Automatically refreshed by the persistence layer on every save. Maps to column: updated_at.
    /// </remarks>
    public DateTimeOffset? UpdatedAt { get; set; }
}
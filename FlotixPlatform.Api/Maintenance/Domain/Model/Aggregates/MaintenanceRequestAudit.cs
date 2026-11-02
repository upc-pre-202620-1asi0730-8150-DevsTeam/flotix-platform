using FlotixPlatform.API.Shared.Domain.Model;

namespace FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;

/// <summary>
///     Audit extension for MaintenanceRequest aggregate.
/// </summary>
/// <remarks>
///     This partial class extends MaintenanceRequest with audit trail properties.
///     CreatedAt and UpdatedAt are automatically managed by the persistence layer
///     via the AuditableEntityInterceptor in the Shared bounded context.
/// </remarks>
public partial class MaintenanceRequest : IAuditableEntity
{
    /// <summary>
    ///     Gets the timestamp when this maintenance request was created.
    /// </summary>
    /// <remarks>
    ///     Automatically set once by the persistence layer on first insert. Maps to column: created_at.
    /// </remarks>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets the timestamp when this maintenance request was last updated.
    /// </summary>
    /// <remarks>
    ///     Automatically refreshed by the persistence layer on every save. Maps to column: updated_at.
    /// </remarks>
    public DateTimeOffset? UpdatedAt { get; set; }
}

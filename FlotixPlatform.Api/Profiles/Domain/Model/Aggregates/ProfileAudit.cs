using FlotixPlatform.Api.Shared.Domain.Model.Entities;

namespace FlotixPlatform.Api.Profiles.Domain.Model.Aggregates;

public partial class Profile : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

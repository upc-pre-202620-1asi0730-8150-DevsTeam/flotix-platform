using FlotixPlatform.Api.Shared.Domain.Model.Entities;

namespace FlotixPlatform.Api.Iam.Domain.Model.Aggregates;

public partial class User : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
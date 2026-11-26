using FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;

namespace FlotixPlatform.Api.Profiles.Domain.Model.Queries;

/// <summary>
///     Get Profile by Id Query
/// </summary>
/// <param name="ProfileId">
///     The <see cref="ProfileId" /> of the profile to retrieve
/// </param>
public record GetProfileByIdQuery(ProfileId ProfileId)
{
    public GetProfileByIdQuery(int profileId) : this(new ProfileId(profileId))
    {
    }
}

using FlotixPlatform.Api.Profiles.Application.CommandServices;
using FlotixPlatform.Api.Profiles.Application.QueryServices;
using FlotixPlatform.Api.Profiles.Domain.Model.Commands;
using FlotixPlatform.Api.Profiles.Domain.Model.Queries;
using FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Profiles.Interfaces.Acl;

namespace FlotixPlatform.Api.Profiles.Application.Acl;

/// <summary>
///     Facade for the profiles context
/// </summary>
/// <param name="profileCommandService">
///     The profile command service
/// </param>
/// <param name="profileQueryService">
///     The profile query service
/// </param>
public class ProfilesContextFacade(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService
) : IProfilesContextFacade
{
    // inheritedDoc
    public async Task<int> CreateProfile(string firstName, string lastName, string email, string role,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<EProfileRole>(role, true, out var profileRole)) return 0;
        var createProfileCommand = new CreateProfileCommand(firstName, lastName, email, profileRole);
        var result = await profileCommandService.Handle(createProfileCommand, cancellationToken);
        return result.Value?.Id.Value ?? 0;
    }

    // inheritedDoc
    public async Task<int> FetchProfileIdByEmail(string email, CancellationToken cancellationToken)
    {
        var getProfileByEmailQuery = new GetProfileByEmailQuery(new EmailAddress(email));
        var profile = await profileQueryService.Handle(getProfileByEmailQuery, cancellationToken);
        return profile?.Id.Value ?? 0;
    }

    // inheritedDoc
    public async Task<bool> ExistsProfileById(int profileId, CancellationToken cancellationToken)
    {
        var getProfileByIdQuery = new GetProfileByIdQuery(profileId);
        var profile = await profileQueryService.Handle(getProfileByIdQuery, cancellationToken);
        return profile is not null;
    }
}

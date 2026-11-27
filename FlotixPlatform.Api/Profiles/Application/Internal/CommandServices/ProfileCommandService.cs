using FlotixPlatform.Api.Profiles.Application.CommandServices;
using FlotixPlatform.Api.Profiles.Domain.Model;
using FlotixPlatform.Api.Profiles.Domain.Model.Aggregates;
using FlotixPlatform.Api.Profiles.Domain.Model.Commands;
using FlotixPlatform.Api.Profiles.Domain.Model.Errors;
using FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Profiles.Domain.Repositories;
using FlotixPlatform.Api.Shared.Application.Model;
using FlotixPlatform.Api.Shared.Domain.Repositories;

namespace FlotixPlatform.Api.Profiles.Application.Internal.CommandServices;

/// <summary>
///     Profile command service
/// </summary>
/// <param name="profileRepository">
///     Profile repository
/// </param>
/// <param name="unitOfWork">
///     Unit of work
/// </param>
public class ProfileCommandService(
    IProfileRepository profileRepository,
    IUnitOfWork unitOfWork)
    : IProfileCommandService
{
    /// <inheritdoc />
    public async Task<Result<Profile>> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        if (command.Plan is not null && command.Role != EProfileRole.Owner)
            return Result<Profile>.Failure(ProfileErrors.PlanRequiresOwnerRole, ProfilesError.InvalidProfileData);

        if (await profileRepository.ExistsByEmailAsync(new EmailAddress(command.Email), cancellationToken))
            return Result<Profile>.Failure(ProfileErrors.EmailAlreadyRegisteredWithEmail(command.Email),
                ProfilesError.EmailAlreadyRegistered);

        var profile = new Profile(command);
        await profileRepository.AddAsync(profile, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<Profile>.Success(profile);
    }
}

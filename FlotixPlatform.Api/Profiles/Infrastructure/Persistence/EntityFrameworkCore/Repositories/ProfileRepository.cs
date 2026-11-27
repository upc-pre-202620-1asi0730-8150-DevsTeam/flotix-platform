using FlotixPlatform.Api.Profiles.Domain.Model.Aggregates;
using FlotixPlatform.Api.Profiles.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Profiles.Domain.Repositories;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Profile repository implementation
/// </summary>
/// <param name="context">
///     The database context
/// </param>
public class ProfileRepository(AppDbContext context)
    : BaseRepository<Profile>(context), IProfileRepository
{
    /// <inheritdoc />
    public async Task<Profile?> FindProfileByEmailAsync(EmailAddress email, CancellationToken cancellationToken)
    {
        return await Context.Set<Profile>().FirstOrDefaultAsync(p => p.Email.Address == email.Address, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Profile?> FindProfileByEmailAsNoTrackingAsync(EmailAddress email,
        CancellationToken cancellationToken)
    {
        return await Context.Set<Profile>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email.Address == email.Address, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Profile?> FindByIdAsNoTrackingAsync(ProfileId id, CancellationToken cancellationToken)
    {
        return await Context.Set<Profile>()
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByEmailAsync(EmailAddress email, CancellationToken cancellationToken)
    {
        return await Context.Set<Profile>().AnyAsync(p => p.Email.Address == email.Address, cancellationToken);
    }
}

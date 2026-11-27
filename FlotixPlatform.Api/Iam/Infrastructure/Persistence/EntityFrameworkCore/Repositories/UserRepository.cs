using FlotixPlatform.Api.Iam.Domain.Model.Aggregates;
using FlotixPlatform.Api.Iam.Domain.Model.ValueObjects;
using FlotixPlatform.Api.Iam.Domain.Repositories;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.Api.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core implementation of the IAM user repository.
/// </summary>
/// <param name="context">The application database context.</param>
public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Set<User>().AddAsync(user, cancellationToken);
    }

    public async Task<User?> FindByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await context.Set<User>().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> FindByUsernameAsNoTrackingAsync(string username, CancellationToken cancellationToken)
    {
        return await context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<User?> FindByIdAsNoTrackingAsync(UserId id, CancellationToken cancellationToken)
    {
        return await context.Set<User>()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<User>> ListAsNoTrackingAsync(CancellationToken cancellationToken)
    {
        return await context.Set<User>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await context.Set<User>().AnyAsync(u => u.Username == username, cancellationToken);
    }

    public void Update(User user) => context.Set<User>().Update(user);

    public void Remove(User user) => context.Set<User>().Remove(user);
}

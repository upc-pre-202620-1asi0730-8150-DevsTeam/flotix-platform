namespace FlotixPlatform.Api.DemoData.Application.Services;

/// <summary>
///     Generates the demo fleet (about three months of history) of an account.
/// </summary>
public interface IDemoDataSeeder
{
    /// <summary>
    ///     Generates the demo data of an IAM user when it is enabled and the account does not have it yet.
    /// </summary>
    /// <param name="userId">The IAM user id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The number of generated rows, or 0 when nothing was generated.</returns>
    Task<int> SeedForUserAsync(int userId, CancellationToken cancellationToken = default);
}
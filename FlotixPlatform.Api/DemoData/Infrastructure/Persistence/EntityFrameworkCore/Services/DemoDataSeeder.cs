using FlotixPlatform.Api.DemoData.Application.Services;
using FlotixPlatform.Api.DemoData.Infrastructure.Configuration;
using FlotixPlatform.Api.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FlotixPlatform.Api.DemoData.Infrastructure.Persistence.EntityFrameworkCore.Services;

/// <summary>
///     Generates the demo data of an account by calling the <c>public.flotix_seed_demo_data</c> PL/pgSQL function
///     installed by the <c>AddDemoDataFunctions</c> migration.
/// </summary>
public class DemoDataSeeder(
    AppDbContext context,
    IOptions<DemoDataSettings> settings,
    ILogger<DemoDataSeeder> logger) : IDemoDataSeeder
{
    /// <inheritdoc />
    public async Task<int> SeedForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        if (!settings.Value.SeedOnSignUp) return 0;

        var days = settings.Value.HistoryDays;
        var rows = await context.Database
            .SqlQuery<int>($"""SELECT public.flotix_seed_demo_data({userId}, {days}) AS "Value" """)
            .SingleAsync(cancellationToken);

        logger.LogInformation("Generated {Rows} demo data rows ({Days} days of history) for user {UserId}",
            rows, days, userId);
        return rows;
    }
}
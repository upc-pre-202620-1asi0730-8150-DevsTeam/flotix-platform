using System.Text.RegularExpressions;
using Npgsql;

namespace FlotixPlatform.Api.Shared.Infrastructure.Interfaces.Asp.Configuration.Extensions;

/// <summary>
///     Resolves the PostgreSQL (Neon) connection strings from configuration without ever storing secrets in source control.
/// </summary>
/// <remarks>
///     The <c>appsettings*.json</c> files only contain <c>%PLACEHOLDERS%</c> (for example <c>%DATABASE_PASSWORD%</c>)
///     that are expanded with <see cref="Environment.ExpandEnvironmentVariables(string)"/>. The real values come from
///     environment variables (hosting platform) or from a git-ignored <c>.env</c> file (development).
///     Alternatively the whole connection string can be supplied through <c>DATABASE_URL</c> or user-secrets, in either the
///     Neon URI form (<c>postgresql://user:pass@host/db?sslmode=require</c>) or the Npgsql key/value form.
/// </remarks>
public static partial class DatabaseConnectionExtensions
{
    [GeneratedRegex("%(\\w+)%")]
    private static partial Regex PlaceholderRegex();

    /// <summary>
    ///     Gets the connection string used by the application at runtime (Neon pooled endpoint).
    /// </summary>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>An Npgsql key/value connection string.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the connection string or one of its variables is missing.</exception>
    public static string GetDatabaseConnectionString(this IConfiguration configuration)
    {
        var raw = configuration["DATABASE_URL"] ?? configuration.GetConnectionString("DefaultConnection");
        var resolved = Resolve(raw, out var missing);
        if (resolved is null)
            throw new InvalidOperationException(missing.Count > 0
                ? $"Missing environment variable(s) for the database connection: {string.Join(", ", missing)}. " +
                  "Define them in the environment or in a .env file (see .env.example)."
                : "No database connection string configured. Set ConnectionStrings:DefaultConnection, DATABASE_URL or the DATABASE_* variables.");
        return resolved;
    }

    /// <summary>
    ///     Gets the connection string used to apply migrations, or <c>null</c> when none is configured.
    /// </summary>
    /// <remarks>
    ///     Neon recommends the direct (non-pooled) endpoint for migrations because PgBouncer, used by the
    ///     <c>-pooler</c> host, does not support the session-level advisory locks that EF Core takes.
    /// </remarks>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>An Npgsql key/value connection string, or <c>null</c>.</returns>
    public static string? GetMigrationsConnectionString(this IConfiguration configuration) =>
        Resolve(configuration["DATABASE_URL_DIRECT"] ?? configuration.GetConnectionString("MigrationsConnection"), out _);

    private static string? Resolve(string? raw, out List<string> missing)
    {
        missing = [];
        if (string.IsNullOrWhiteSpace(raw)) return null;

        var value = raw.Trim();
        if (value.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) ||
            value.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
            return FromUri(value);

        var expanded = Environment.ExpandEnvironmentVariables(value);
        missing = PlaceholderRegex().Matches(expanded).Select(m => m.Groups[1].Value).Distinct().ToList();
        return missing.Count > 0 ? null : expanded;
    }

    private static string FromUri(string value)
    {
        var uri = new Uri(value);
        var userInfo = uri.UserInfo.Split(':', 2);
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort || uri.Port < 0 ? 5432 : uri.Port,
            Database = uri.AbsolutePath.TrimStart('/'),
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : null,
            SslMode = SslMode.Require,
            Timeout = 30 // Neon computes can take a few seconds to wake up when idle.
        };

        foreach (var pair in uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var kv = pair.Split('=', 2);
            var key = kv[0].ToLowerInvariant();
            var val = kv.Length > 1 ? Uri.UnescapeDataString(kv[1]) : string.Empty;
            switch (key)
            {
                case "sslmode":
                    builder.SslMode = val.ToLowerInvariant() switch
                    {
                        "disable" => SslMode.Disable,
                        "allow" => SslMode.Allow,
                        "prefer" => SslMode.Prefer,
                        "verify-ca" => SslMode.VerifyCA,
                        "verify-full" => SslMode.VerifyFull,
                        _ => SslMode.Require
                    };
                    break;
                case "channel_binding":
                    builder.ChannelBinding = val.ToLowerInvariant() switch
                    {
                        "disable" => ChannelBinding.Disable,
                        "prefer" => ChannelBinding.Prefer,
                        _ => ChannelBinding.Require
                    };
                    break;
            }
        }

        return builder.ConnectionString;
    }
}

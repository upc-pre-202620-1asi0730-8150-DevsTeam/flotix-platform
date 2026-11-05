namespace FlotixPlatform.Api.Shared.Infrastructure.Interfaces.Asp.Configuration.Extensions;

/// <summary>
///     Loads a git-ignored <c>.env</c> file into the process environment during development.
/// </summary>
/// <remarks>
///     Variables already defined in the environment are never overwritten, so real environment variables
///     (hosting platform, CI) always win over the file.
/// </remarks>
public static class DotEnvLoader
{
    /// <summary>
    ///     Loads the <c>.env</c> file of the given directory, if it exists.
    /// </summary>
    /// <param name="directory">The directory that may contain a <c>.env</c> file.</param>
    public static void Load(string directory)
    {
        var path = Path.Combine(directory, ".env");
        if (!File.Exists(path)) return;

        foreach (var line in File.ReadAllLines(path))
        {
            var entry = line.Trim();
            if (entry.Length == 0 || entry.StartsWith('#')) continue;

            var separator = entry.IndexOf('=');
            if (separator <= 0) continue;

            var key = entry[..separator].Trim();
            var value = entry[(separator + 1)..].Trim().Trim('"', '\'');
            if (Environment.GetEnvironmentVariable(key) is null)
                Environment.SetEnvironmentVariable(key, value);
        }
    }
}

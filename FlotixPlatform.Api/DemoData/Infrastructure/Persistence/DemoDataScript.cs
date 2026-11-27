namespace FlotixPlatform.Api.DemoData.Infrastructure.Persistence;

/// <summary>
///     Gives access to the PL/pgSQL script that installs the demo data functions
///     (<c>DemoData/Scripts/flotix_demo_data.sql</c>, embedded in the assembly).
/// </summary>
public static class DemoDataScript
{
    private const string ResourceName = "FlotixPlatform.Api.DemoData.Scripts.flotix_demo_data.sql";

    /// <summary>
    ///     Reads the script that creates <c>public.demo_data_rows</c> and the <c>flotix_*demo_data*</c> functions.
    /// </summary>
    /// <returns>The SQL script.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the script is not embedded in the assembly.</exception>
    public static string Read()
    {
        using var stream = typeof(DemoDataScript).Assembly.GetManifestResourceStream(ResourceName)
                           ?? throw new InvalidOperationException($"Embedded resource '{ResourceName}' was not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
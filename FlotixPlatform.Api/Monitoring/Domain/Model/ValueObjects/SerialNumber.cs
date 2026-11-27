using System.Text.RegularExpressions;

namespace FlotixPlatform.Api.Monitoring.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a validated serial number.
/// </summary>
public sealed partial record SerialNumber
{
    private const int MaxLength = 100;

    /// <summary>
    ///     Initializes a new <see cref="SerialNumber"/> with the given raw string value.
    /// </summary>
    /// <param name="value">The raw serial number. Must be non-null, non-whitespace, at most 100 characters and match the expected format.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null, whitespace, too long or malformed.</exception>
    public SerialNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Serial number cannot be null or whitespace.", nameof(value));

        var normalizedValue = value.Trim().ToUpperInvariant();
        if (normalizedValue.Length > MaxLength)
            throw new ArgumentException($"Serial number cannot exceed {MaxLength} characters.", nameof(value));

        if (!ValidFormatRegex().IsMatch(normalizedValue))
            throw new ArgumentException("Serial number has an invalid format.", nameof(value));

        Value = normalizedValue;
    }

    /// <summary>
    ///     Gets the underlying primitive value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;

    [GeneratedRegex(@"^[A-Z0-9-]+$")]
    private static partial Regex ValidFormatRegex();
}

using System.Text.RegularExpressions;

namespace FlotixPlatform.Api.Fleet.Domain.Model.ValueObjects;

/// <summary>
///     Value object representing a validated license plate.
/// </summary>
public sealed partial record LicensePlate
{
    private const int MaxLength = 15;

    /// <summary>
    ///     Initializes a new <see cref="LicensePlate"/> with the given raw string value.
    /// </summary>
    /// <param name="value">The raw license plate. Must be non-null, non-whitespace, at most 15 characters and match the expected format.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is null, whitespace, too long or malformed.</exception>
    public LicensePlate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("License plate cannot be null or whitespace.", nameof(value));

        var normalizedValue = value.Trim().ToUpperInvariant();
        if (normalizedValue.Length > MaxLength)
            throw new ArgumentException($"License plate cannot exceed {MaxLength} characters.", nameof(value));

        if (!ValidFormatRegex().IsMatch(normalizedValue))
            throw new ArgumentException("License plate has an invalid format.", nameof(value));

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
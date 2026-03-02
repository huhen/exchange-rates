namespace IdentityService.Core.UserAggregate;

[ValueObject<string>]
public partial struct UserPassword
{
    public const int MinLength = 8;
    public const int MaxLength = 64;

    /// <summary>
    /// Validate a password string against UserPassword requirements.
    /// </summary>
    /// <param name="input">The password to validate; may be null or whitespace.</param>
    /// <returns>`Validation.Ok` when the password meets all requirements; otherwise an `Invalid` Validation containing one or more detail messages describing the violation(s) (required, length, or allowed-character errors).</returns>
    private static Validation Validate(string input)
    {
        const string name = nameof(UserPassword);

        Validation? result = null;

        var isNull = string.IsNullOrWhiteSpace(input);

        if (isNull)
            AddError($"{name} is required");

        if (input is { Length: > MaxLength })
            AddError($"{name} must be no more than {MaxLength} characters");

        if (input is { Length: < MinLength })
            AddError($"{name} must contain {MinLength} or more characters");

        if (!isNull && !ValidNameRegex().IsMatch(input))
            AddError($"{name} must contain only printable ASCII characters (letters, digits, symbols, and spaces)");

        return result ?? Validation.Ok;

        void AddError(string message)
        {
            result ??= Validation.Invalid($"{name} does not meet requirements");
            result.WithData(message, string.Empty);
        }
    }

    /// <summary>
    /// Provides a compiled regular expression that matches only printable ASCII characters (U+0020 to U+007E) for an entire string.
    /// </summary>
    /// <returns>A <see cref="Regex"/> that matches one or more printable ASCII characters (space through tilde) anchored to the start and end of the string.</returns>
    [GeneratedRegex(@"^[\x20-\x7E]+$")]
    private static partial Regex ValidNameRegex();
}

namespace IdentityService.Core.UserAggregate;

[ValueObject<string>]
public partial struct UserName
{
    public const int MinLength = 2;
    public const int MaxLength = 64;

    /// <summary>
    /// Validates a username string against the UserName rules: required, length limits, and allowed characters.
    /// </summary>
    /// <param name="input">The username to validate.</param>
    /// <returns>`Validation.Ok` if the input meets all requirements; otherwise an invalid `Validation` containing a primary failure message and one or more specific error messages describing which rules failed.</returns>

    private static Validation Validate(string input)
    {
        const string name = nameof(UserName);
        
        Validation? result = null;

        var isNull = string.IsNullOrWhiteSpace(input);

        if (isNull)
            AddError($"{name} is required");

        if (input is { Length: > MaxLength })
            AddError($"{name} must be no more than {MaxLength} characters");

        if (input is { Length: < MinLength })
            AddError($"{name} must contain {MinLength} or more characters");

        if (!isNull && !ValidNameRegex().IsMatch(input))
            AddError($"{name} must contain only lowercase ASCII letters (a–z), digits (0–9), hyphen, and period");

        return result ?? Validation.Ok;

        void AddError(string message)
        {
            result ??= Validation.Invalid($"{name} does not meet requirements");
            result.WithData(message, string.Empty);
        }
    }

    /// <summary>
    /// Gets a Regex that matches one or more lowercase ASCII letters, digits, hyphen, or period.
    /// </summary>
    /// <returns>A Regex matching the pattern "^[a-z0-9.-]+$".</returns>
    [GeneratedRegex("^[a-z0-9.-]+$")]
    private static partial Regex ValidNameRegex();
}

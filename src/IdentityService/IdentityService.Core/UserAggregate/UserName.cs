using System.Text.RegularExpressions;

namespace IdentityService.Core.UserAggregate;

[ValueObject<string>]
public partial struct UserName
{
    public const int MinLength = 2;
    public const int MaxLength = 63;

    // private static string NormalizeInput(string input) => input.ToLowerInvariant();

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
            AddError($"{name} must contain only lowercase ASCII letters (a–z), hyphen, and period");

        return result ?? Validation.Ok;

        void AddError(string message)
        {
            result ??= Validation.Invalid($"{name} does not meet requirements");
            result.WithData(message, string.Empty);
        }
    }

    [GeneratedRegex("^[a-z.-]+$")]
    private static partial Regex ValidNameRegex();
}

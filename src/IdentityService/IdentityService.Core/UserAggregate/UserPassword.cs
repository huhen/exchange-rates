using System.Text.RegularExpressions;

namespace IdentityService.Core.UserAggregate;

[ValueObject<string>]
public partial struct UserPassword
{
    public const int MinLength = 8;
    public const int MaxLength = 64;

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
            AddError($"{name} must contain only printable ASCII characters (letters, digits, and symbols)");

        return result ?? Validation.Ok;

        void AddError(string message)
        {
            result ??= Validation.Invalid($"{name} does not meet requirements");
            result.WithData(message, string.Empty);
        }
    }

    [GeneratedRegex(@"^[\x21-\x7E]+$")]
    private static partial Regex ValidNameRegex();
}

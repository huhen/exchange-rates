namespace IdentityService.Core.UserAggregate;

[ValueObject<string>]
public partial struct UserPasswordHash
{
    public const int MaxLength = 64;

    /// <summary>
    /// Validates the input string against UserPasswordHash requirements (presence and maximum length).
    /// </summary>
    /// <param name="input">The string to validate as a UserPasswordHash; may be null or whitespace.</param>
    /// <returns>A <see cref="Validation"/> that is <c>Validation.Ok</c> when valid, or <c>Validation.Invalid</c> containing one or more attached error messages when invalid.</returns>
    private static Validation Validate(string input)
    {
        const string name = nameof(UserPasswordHash);

        Validation? result = null;

        var isNull = string.IsNullOrWhiteSpace(input);

        if (isNull)
            AddError($"{name} is required");

        if (input is { Length: > MaxLength })
            AddError($"{name} must be no more than {MaxLength} characters");

        return result ?? Validation.Ok;

        void AddError(string message)
        {
            result ??= Validation.Invalid($"{name} does not meet requirements");
            result.WithData(message, string.Empty);
        }
    }
}

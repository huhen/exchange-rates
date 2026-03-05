namespace CurrencyService.Core.CurrencyAggregate;

[ValueObject<string>]
public partial struct CurrencyCharCode
{
    public const int MaxLength = 3;

    /// <summary>
            /// Validates that the provided currency code consists of exactly three uppercase letters (A-Z).
            /// </summary>
            /// <param name="value">The currency code to validate.</param>
            /// <returns>`Validation.Ok` if <paramref name="value"/> matches three uppercase ASCII letters; `Validation.Invalid` with the message "CurrencyCharCode must contain exactly 3 uppercase letters (A-Z)" otherwise.</returns>
            private static Validation Validate(string value)
        => ValidCharCodeRegex().IsMatch(value)
            ? Validation.Ok
            : Validation.Invalid($"{nameof(CurrencyCharCode)} must contain exactly 3 uppercase letters (A-Z)");

    /// <summary>
    /// Provides the compiled regular expression that matches exactly three uppercase ASCII letters.
    /// </summary>
    /// <returns>A <see cref="Regex"/> that matches strings of exactly three uppercase letters (A-Z).</returns>
    [GeneratedRegex("^[A-Z]{3}$")]
    private static partial Regex ValidCharCodeRegex();
}

namespace CurrencyService.Core.CurrencyAggregate;

[ValueObject<string>]
public partial struct CurrencyName
{
    public const int MaxLength = 128;

    /// <summary>
            /// Validates a currency name string against the maximum allowed length.
            /// </summary>
            /// <param name="value">The currency name to validate.</param>
            /// <returns>`Validation.Invalid` with an error message when <paramref name="value"/> exceeds <c>MaxLength</c> characters; otherwise `Validation.Ok`.</returns>
            private static Validation Validate(string value)
        => value.Length > MaxLength
            ? Validation.Invalid($"{nameof(CurrencyName)} must be no more than {MaxLength} characters")
            : Validation.Ok;
}

namespace CurrencyService.Core.CurrencyAggregate;

[ValueObject<decimal>]
public partial struct CurrencyRate
{
    /// <summary>
    /// Validates that the provided decimal is an acceptable CurrencyRate.
    /// </summary>
    /// <param name="value">The decimal value to validate as a currency rate.</param>
    /// <returns>`Validation.Ok` if <paramref name="value"/> is greater than zero, `Validation.Invalid` with an explanatory message otherwise.</returns>
    private static Validation Validate(decimal value)
        => value > 0
            ? Validation.Ok
            : Validation.Invalid($"{nameof(CurrencyRate)} must be a positive finite number");
}

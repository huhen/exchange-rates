namespace CurrencyService.Core.CurrencyAggregate;

[ValueObject<string>]
public partial struct CurrencyCharCode
{
    public const int MaxLength = 3;

    private static Validation Validate(string value)
        => ValidCharCodeRegex().IsMatch(value)
            ? Validation.Ok
            : Validation.Invalid($"{nameof(CurrencyCharCode)} must contain exactly 3 uppercase letters (A-Z)");

    [GeneratedRegex("^[A-Z]{3}$")]
    private static partial Regex ValidCharCodeRegex();
}

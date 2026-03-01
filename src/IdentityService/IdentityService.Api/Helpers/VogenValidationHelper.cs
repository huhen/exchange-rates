using Microsoft.AspNetCore.Http.HttpResults;
using Vogen;

namespace IdentityService.Api.Helpers;

public static class VogenValidationHelper
{
    /// <summary>
    /// Collects validation errors from a value-object result and produces a ValidationProblem when any errors are present.
    /// </summary>
    /// <param name="r1">The result of creating a value object; if it indicates failure its error messages are added under <paramref name="f1"/>.</param>
    /// <param name="f1">The field name under which to group any errors from <paramref name="r1"/>.</param>
    /// <returns>A <see cref="ValidationProblem"/> mapping <paramref name="f1"/> to the error messages if <paramref name="r1"/> failed; otherwise <c>null</c>.</returns>
    public static ValidationProblem? Validate<T1>(
        ValueObjectOrError<T1> r1, string f1)
    {
        Dictionary<string, string[]>? errors = null;

        if (!r1.IsSuccess)
            AddMessages(ref errors, f1, r1.Error);

        return GetResult(errors);
    }

    /// <summary>
    /// Aggregates validation results from two value-object operations and produces a validation problem if any failed.
    /// </summary>
    /// <param name="r1">The validation result for the first value object.</param>
    /// <param name="f1">The field name to associate with errors from <paramref name="r1"/>.</param>
    /// <param name="r2">The validation result for the second value object.</param>
    /// <param name="f2">The field name to associate with errors from <paramref name="r2"/>.</param>
    /// <returns>A <see cref="ValidationProblem"/> containing error messages keyed by field name if one or more validations failed; otherwise <c>null</c>.</returns>
    public static ValidationProblem? Validate<T1, T2>(
        ValueObjectOrError<T1> r1, string f1,
        ValueObjectOrError<T2> r2, string f2)
    {
        Dictionary<string, string[]>? errors = null;

        if (!r1.IsSuccess)
            AddMessages(ref errors, f1, r1.Error);

        if (!r2.IsSuccess)
            AddMessages(ref errors, f2, r2.Error);

        return GetResult(errors);
    }

    /// <summary>
    /// Aggregates validation errors from three ValueObjectOrError results and returns a ValidationProblem when any errors are present.
    /// </summary>
    /// <param name="r1">The first ValueObjectOrError to inspect for validation errors.</param>
    /// <param name="f1">The field name used as the key for errors from <paramref name="r1"/>.</param>
    /// <param name="r2">The second ValueObjectOrError to inspect for validation errors.</param>
    /// <param name="f2">The field name used as the key for errors from <paramref name="r2"/>.</param>
    /// <param name="r3">The third ValueObjectOrError to inspect for validation errors.</param>
    /// <param name="f3">The field name used as the key for errors from <paramref name="r3"/>.</param>
    /// <returns>A <see cref="ValidationProblem"/> containing collected error messages keyed by the provided field names, or <c>null</c> if all inputs succeeded.</returns>
    public static ValidationProblem? Validate<T1, T2, T3>(
        ValueObjectOrError<T1> r1, string f1,
        ValueObjectOrError<T2> r2, string f2,
        ValueObjectOrError<T3> r3, string f3)
    {
        Dictionary<string, string[]>? errors = null;

        if (!r1.IsSuccess)
            AddMessages(ref errors, f1, r1.Error);

        if (!r2.IsSuccess)
            AddMessages(ref errors, f2, r2.Error);

        if (!r3.IsSuccess)
            AddMessages(ref errors, f3, r3.Error);

        return GetResult(errors);
    }

    /// <summary>
    /// Aggregate validation errors from up to four value-object results into an endpoint-compatible result.
    /// </summary>
    /// <param name="r1">Validation result for the first value object.</param>
    /// <param name="f1">Field name to associate with errors from <paramref name="r1"/>.</param>
    /// <param name="r2">Validation result for the second value object.</param>
    /// <param name="f2">Field name to associate with errors from <paramref name="r2"/>.</param>
    /// <param name="r3">Validation result for the third value object.</param>
    /// <param name="f3">Field name to associate with errors from <paramref name="r3"/>.</param>
    /// <param name="r4">Validation result for the fourth value object.</param>
    /// <param name="f4">Field name to associate with errors from <paramref name="r4"/>.</param>
    /// <returns>A ValidationProblem mapped by field names when any input failed, or `null` if all inputs succeeded.</returns>
    public static IResult? Validate<T1, T2, T3, T4>(
        ValueObjectOrError<T1> r1, string f1,
        ValueObjectOrError<T2> r2, string f2,
        ValueObjectOrError<T3> r3, string f3,
        ValueObjectOrError<T4> r4, string f4)
    {
        Dictionary<string, string[]>? errors = null;

        if (!r1.IsSuccess)
            AddMessages(ref errors, f1, r1.Error);

        if (!r2.IsSuccess)
            AddMessages(ref errors, f2, r2.Error);

        if (!r3.IsSuccess)
            AddMessages(ref errors, f3, r3.Error);

        if (!r4.IsSuccess)
            AddMessages(ref errors, f4, r4.Error);

        return GetResult(errors);
    }

    /// <summary>
            /// Converts collected field errors into a ValidationProblem response if any errors exist.
            /// </summary>
            /// <param name="errors">Optional dictionary mapping field names to their validation messages.</param>
            /// <returns>A ValidationProblem built from <paramref name="errors"/>, or <c>null</c> if the dictionary is empty or <c>null</c>.</returns>
            private static ValidationProblem? GetResult(Dictionary<string, string[]>? errors) =>
        errors?.Count > 0
            ? TypedResults.ValidationProblem(errors)
            : null;

    /// <summary>
    /// Ensures the provided errors dictionary is initialized and sets the entry for the specified field to an array containing the validation message followed by any keys from the validation's Data.
    /// </summary>
    /// <param name="errors">Reference to the errors dictionary to populate; will be created if null.</param>
    /// <param name="fieldName">The field name to use as the dictionary key.</param>
    /// <param name="error">The validation instance whose ErrorMessage and Data keys are added to the dictionary entry.</param>
    private static void AddMessages(ref Dictionary<string, string[]>? errors, string fieldName, Validation error)
    {
        errors ??= new Dictionary<string, string[]>();

        errors[fieldName] =
        [
            error.ErrorMessage,
            ..error.Data?.Keys.Cast<string>() ?? []
        ];
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Vogen;

namespace IdentityService.Api.Helpers;

public static class VogenValidationHelper
{
    public static ValidationProblem? Validate<T1>(
        ValueObjectOrError<T1> r1, string f1)
    {
        Dictionary<string, string[]>? errors = null;

        if (!r1.IsSuccess)
            AddMessages(ref errors, f1, r1.Error);

        return GetResult(errors);
    }

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

    private static ValidationProblem? GetResult(Dictionary<string, string[]>? errors) =>
        errors?.Count > 0
            ? TypedResults.ValidationProblem(errors)
            : null;

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

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Validated.Enums;

namespace Validated.Utilities;

public static class ValidationHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Compare<T>(T left, T right, ComparisonOperator op)
    {
        if (op == ComparisonOperator.Equal)
            return EqualityComparer<T>.Default.Equals(left, right);

        if (op == ComparisonOperator.NotEqual)
            return !EqualityComparer<T>.Default.Equals(left, right);

        int result = Comparer<T>.Default.Compare(left, right);
        return op switch
        {
            ComparisonOperator.GreaterThan => result > 0,
            ComparisonOperator.GreaterThanOrEqual => result >= 0,
            ComparisonOperator.LessThan => result < 0,
            ComparisonOperator.LessThanOrEqual => result <= 0,
            _ => true
        };
    }

    public static bool IsCollectionValid<T>(IEnumerable<T>? collection, Func<T, bool> isValidPredicate)
    {
        if (collection == null) return true;
        foreach (var item in collection)
        {
            if (item != null && !isValidPredicate(item)) return false;
        }
        return true;
    }

    public static void ValidateCollection<T>(
        IEnumerable<T>? collection,
        string propertyName,
        List<ValidationError> errors,
        Func<T, ValidationResult> validateAction)
    {
        if (collection == null) return;

        int index = 0;
        foreach (var item in collection)
        {
            if (item != null)
            {
                var result = validateAction(item);
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        string fullPath = string.IsNullOrEmpty(error.PropertyName)
                            ? $"{propertyName}[{index}]"
                            : $"{propertyName}[{index}].{error.PropertyName}";

                        errors.Add(new ValidationError(fullPath, error.Message, error.RuleName));
                    }
                }
            }
            index++;
        }
    }

    public static bool TryValidateCollection<T>(
        IEnumerable<T>? collection,
        string propertyName,
        out ValidationError error,
        Func<T, ValidationResult> validateAction)
    {
        if (collection == null)
        {
            error = default;
            return true;
        }

        int index = 0;
        foreach (var item in collection)
        {
            if (item != null)
            {
                var result = validateAction(item);
                if (!result.IsValid)
                {
                    string fullPath = string.IsNullOrEmpty(result.Errors[0].PropertyName)
                        ? $"{propertyName}[{index}]"
                        : $"{propertyName}[{index}].{result.Errors[0].PropertyName}";
                    error = new ValidationError(fullPath, result.Errors[0].Message, result.Errors[0].RuleName);
                    return false;
                }
            }
            index++;
        }

        error = default;
        return true;
    }
}

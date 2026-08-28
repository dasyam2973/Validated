namespace Validated.Generator.Constants;

internal static class ValidationErrorMessages
{
    internal static string NotNull(string propertyName)
        => $"'{propertyName}' must not be null.";

    internal static string NotEmpty(string propertyName)
        => $"'{propertyName}' must not be empty.";

    internal static string StringLength(string propertyName, int min, int max)
        => $"'{propertyName}' must be between {min} and {max} characters.";

    internal static string MinLength(string propertyName, int min)
        => $"'{propertyName}' must be at least {min} characters long.";

    internal static string MaxLength(string propertyName, int max)
        => $"'{propertyName}' must be {max} characters or fewer.";

    internal static string Length(string propertyName, int min, int max)
    {
        if (min == max)
            return $"'{propertyName}' must be exactly {max} in length.";

        if (min <= 0)
            return $"'{propertyName}' must be at most {max} in length.";

        if (max == int.MaxValue)
            return $"'{propertyName}' must be at least {min} in length.";

        return $"'{propertyName}' must be between {min} and {max} in length.";
    }

    internal static string Range<T>(string propertyName, T min, T max)
        => $"'{propertyName}' must be between {min} and {max}.";

    internal static string GreaterThan<T>(string propertyName, T value)
        => $"'{propertyName}' must be greater than {value}.";

    internal static string GreaterThanOrEqual<T>(string propertyName, T value)
        => $"'{propertyName}' must be greater than or equal to {value}.";

    internal static string LessThan<T>(string propertyName, T value)
        => $"'{propertyName}' must be less than {value}.";

    internal static string LessThanOrEqual<T>(string propertyName, T value)
        => $"'{propertyName}' must be less than or equal to {value}.";

    internal static string GreaterThanOrEqualProperty(string propertyName, string otherPropertyName)
        => $"'{propertyName}' must be greater than or equal to '{otherPropertyName}'.";

    internal static string GreaterThanProperty(string propertyName, string otherPropertyName)
        => $"'{propertyName}' must be greater than '{otherPropertyName}'.";

    internal static string LessThanOrEqualProperty(string propertyName, string otherPropertyName)
        => $"'{propertyName}' must be less than or equal to '{otherPropertyName}'.";

    internal static string LessThanProperty(string propertyName, string otherPropertyName)
        => $"'{propertyName}' must be less than '{otherPropertyName}'.";

    internal static string EqualProperty(string propertyName, string otherPropertyName)
        => $"'{propertyName}' must be equal to '{otherPropertyName}'.";

    internal static string NotEqualProperty(string propertyName, string otherPropertyName)
        => $"'{propertyName}' must not be equal to '{otherPropertyName}'.";

    internal static string Regex(string propertyName)
        => $"'{propertyName}' is not in the correct format.";

    internal static string Email(string propertyName)
        => $"'{propertyName}' is not a valid email address.";
}
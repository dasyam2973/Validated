using Args = Validated.MessageArguments;

namespace Validated;

public static class ValidationErrorMessages
{
    public const string NotNull = $"'{{{Args.PropertyName}}}' must not be null.";
    public const string NotEmpty = $"'{{{Args.PropertyName}}}' must not be empty.";

    public const string ExactLength = $"'{{{Args.PropertyName}}}' must be exactly {{{Args.Max}}} in length.";
    public const string MaxLength = $"'{{{Args.PropertyName}}}' must be {{{Args.Max}}} characters or fewer.";
    public const string MinLength = $"'{{{Args.PropertyName}}}' must be at least {{{Args.Min}}} characters long.";
    public const string LengthRange = $"'{{{Args.PropertyName}}}' must be between {{{Args.Min}}} and {{{Args.Max}}} in length.";

    public const string Range = $"'{{{Args.PropertyName}}}' must be between {{{Args.Min}}} and {{{Args.Max}}}.";
    public const string GreaterThan = $"'{{{Args.PropertyName}}}' must be greater than {{{Args.Value}}}.";
    public const string GreaterThanOrEqual = $"'{{{Args.PropertyName}}}' must be greater than or equal to {{{Args.Value}}}.";
    public const string LessThan = $"'{{{Args.PropertyName}}}' must be less than {{{Args.Value}}}.";
    public const string LessThanOrEqual = $"'{{{Args.PropertyName}}}' must be less than or equal to {{{Args.Value}}}.";
    public const string Equal = $"'{{{Args.PropertyName}}}' must be equal to '{{{Args.Value}}}'.";
    public const string NotEqual = $"'{{{Args.PropertyName}}}' must not be equal to '{{{Args.Value}}}'.";

    public const string GreaterThanProperty = $"'{{{Args.PropertyName}}}' must be greater than '{{{Args.OtherPropertyName}}}'.";
    public const string GreaterThanOrEqualProperty = $"'{{{Args.PropertyName}}}' must be greater than or equal to '{{{Args.OtherPropertyName}}}'.";
    public const string LessThanProperty = $"'{{{Args.PropertyName}}}' must be less than '{{{Args.OtherPropertyName}}}'.";
    public const string LessThanOrEqualProperty = $"'{{{Args.PropertyName}}}' must be less than or equal to '{{{Args.OtherPropertyName}}}'.";
    public const string EqualProperty = $"'{{{Args.PropertyName}}}' must be equal to '{{{Args.OtherPropertyName}}}'.";
    public const string NotEqualProperty = $"'{{{Args.PropertyName}}}' must not be equal to '{{{Args.OtherPropertyName}}}'.";

    public const string Regex = $"'{{{Args.PropertyName}}}' is not in the correct format.";
}
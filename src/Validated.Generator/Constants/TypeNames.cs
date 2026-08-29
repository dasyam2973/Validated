namespace Validated.Generator.Constants;

internal static class TypeNames
{
    public const string ValidatedNamespace = "Validated";
    public const string AnnotationsNamespace = "Validated.Annotations";

    public const string ValidationMessageHelpersFqn = "global::Validated.ValidationMessageHelpers";
    public const string ValidationErrorMessagesFqn = "global::Validated.ValidationErrorMessages";
    public const string ValidationErrorFqn = "global::Validated.ValidationError";

    public const string ValidatableAttribute = "ValidatableAttribute";
    public const string ValidatableAttributeFqn = "Validated.Annotations.ValidatableAttribute";

    public const string VStringLength = "VStringLengthAttribute";
    public const string VStringLengthFqn = "Validated.Annotations.VStringLengthAttribute";

    public const string VLength = "VLengthAttribute";
    public const string VLengthFqn = "Validated.Annotations.VLengthAttribute";

    public const string VNotNull = "VNotNullAttribute";
    public const string VNotNullFqn = "Validated.Annotations.VNotNullAttribute";

    public const string VNotEmpty = "VNotEmptyAttribute";
    public const string VNotEmptyFqn = "Validated.Annotations.VNotEmptyAttribute";

    public const string VRange = "VRangeAttribute";
    public const string VRangeFqn = "Validated.Annotations.VRangeAttribute";

    public const string VRegex = "VRegexAttribute";
    public const string VRegexFqn = "Validated.Annotations.VRegexAttribute";

    #region Value Comparison
    public const string VGreaterThan = "VGreaterThanAttribute";
    public const string VGreaterThanFqn = "Validated.Annotations.VGreaterThanAttribute";

    public const string VGreaterThanOrEqual = "VGreaterThanOrEqualAttribute";
    public const string VGreaterThanOrEqualFqn = "Validated.Annotations.VGreaterThanOrEqualAttribute";

    public const string VLessThan = "VLessThanAttribute";
    public const string VLessThanFqn = "Validated.Annotations.VLessThanAttribute";

    public const string VLessThanOrEqual = "VLessThanOrEqualAttribute";
    public const string VLessThanOrEqualFqn = "Validated.Annotations.VLessThanOrEqualAttribute";

    public const string VEqual = "VEqualAttribute";
    public const string VEqualFqn = "Validated.Annotations.VEqualAttribute";

    public const string VNotEqual = "VNotEqualAttribute";
    public const string VNotEqualFqn = "Validated.Annotations.VNotEqualAttribute";
    #endregion

    #region Property Comparison
    public const string VGreaterThanProperty = "VGreaterThanPropertyAttribute";
    public const string VGreaterThanPropertyFqn = "Validated.Annotations.VGreaterThanPropertyAttribute";

    public const string VGreaterThanOrEqualProperty = "VGreaterThanOrEqualPropertyAttribute";
    public const string VGreaterThanOrEqualPropertyFqn = "Validated.Annotations.VGreaterThanOrEqualPropertyAttribute";

    public const string VLessThanProperty = "VLessThanPropertyAttribute";
    public const string VLessThanPropertyFqn = "Validated.Annotations.VLessThanPropertyAttribute";

    public const string VLessThanOrEqualProperty = "VLessThanOrEqualPropertyAttribute";
    public const string VLessThanOrEqualPropertyFqn = "Validated.Annotations.VLessThanOrEqualPropertyAttribute";

    public const string VEqualProperty = "VEqualPropertyAttribute";
    public const string VEqualPropertyFqn = "Validated.Annotations.VEqualPropertyAttribute";

    public const string VNotEqualProperty = "VNotEqualPropertyAttribute";
    public const string VNotEqualPropertyFqn = "Validated.Annotations.VNotEqualPropertyAttribute";
    #endregion
}

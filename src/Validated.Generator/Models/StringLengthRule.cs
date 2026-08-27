using Microsoft.CodeAnalysis;
using Validated.Generator.Constants;

namespace Validated.Generator.Models;

public sealed class StringLengthRule : ValidationRule
{
    public int Min { get; }
    public int Max { get; }
    public string? CustomErrorMessage { get; }

    public StringLengthRule(int min, int max, string? customErrorMessage = null)
    {
        Min = min;
        Max = max;
        CustomErrorMessage = customErrorMessage;
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return $"({targetProperty} is null || {targetProperty}.Length >= {Min} && {targetProperty}.Length <= {Max})";
    }

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"({targetProperty} is not null && ({targetProperty}.Length < {Min} || {targetProperty}.Length > {Max}))";

        string defaultMessage = ValidationErrorMessages.StringLength(propertyName, Min, Max);
        string finalMessage = !string.IsNullOrWhiteSpace(CustomErrorMessage)
            ? CustomErrorMessage!
                .Replace("{0}", propertyName)
                .Replace("{1}", Min.ToString())
                .Replace("{2}", Max.ToString())
            : defaultMessage;

        string errorExpression = $"new global::Validated.ValidationError(nameof({targetProperty}), \"{finalMessage}\", \"StringLength\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is StringLengthRule otherRule &&
            otherRule.Min == Min &&
            otherRule.Max == Max &&
            string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

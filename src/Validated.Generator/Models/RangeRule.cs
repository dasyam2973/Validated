using Microsoft.CodeAnalysis;
using Validated.Generator.Constants;

namespace Validated.Generator.Models;

public sealed class RangeRule : ValidationRule
{
    public double Min { get; }
    public double Max { get; }
    public string? CustomErrorMessage { get; }

    public RangeRule(double min, double max, string? customErrorMessage = null)
    {
        Min = min;
        Max = max;
        CustomErrorMessage = customErrorMessage;
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return $"({targetProperty} == null || {targetProperty} >= {Min} && {targetProperty} <= {Max})";
    }

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"({targetProperty} != null && ({targetProperty} < {Min} || {targetProperty} > {Max}))";

        string defaultMessage = ValidationErrorMessages.Range(propertyName, Min, Max);
        string finalMessage = !string.IsNullOrWhiteSpace(CustomErrorMessage)
            ? CustomErrorMessage!
                .Replace("{0}", propertyName)
                .Replace("{1}", Min.ToString())
                .Replace("{2}", Max.ToString())
            : defaultMessage;

        string errorExpression = $"new global::Validated.ValidationError(nameof({targetProperty}), \"{finalMessage}\", \"Range\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is RangeRule otherRule &&
            otherRule.Min == Min &&
            otherRule.Max == Max &&
            string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

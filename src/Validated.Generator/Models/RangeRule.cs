using Microsoft.CodeAnalysis;
using Validated.Generator.Constants;
using Validated.Generator.Utilities;

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

        string errorMessage = new MessageFormatter()
            .With(MessageArguments.PropertyName, propertyName)
            .With(MessageArguments.Min, Min)
            .With(MessageArguments.Max, Max)
            .Format(ValidationErrorMessages.Range);

        string errorExpression = $"new global::Validated.ValidationError(\"{propertyName}\", \"{errorMessage}\", \"Range\")";

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

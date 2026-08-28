using System;
using Validated.Generator.Constants;
using Validated.Generator.Enums;

namespace Validated.Generator.Models;

public sealed class LengthRule : ValidationRule
{
    public ValidationTargetKind TargetKind { get; }
    public int Min { get; }
    public int Max { get; }
    public string? CustomErrorMessage { get; }

    public LengthRule(ValidationTargetKind targetKind, int min, int max, string? customErrorMessage = null)
    {
        if (targetKind is not (ValidationTargetKind.String or ValidationTargetKind.Array or ValidationTargetKind.Collection))
        {
            throw new ArgumentException($"Length validation rule does not support {targetKind}.", nameof(targetKind));
        }

        TargetKind = targetKind;
        Min = min;
        Max = max;
        CustomErrorMessage = customErrorMessage;
    }

    private string GetLengthAccessor(string targetProperty) => TargetKind switch
    {
        ValidationTargetKind.String => $"{targetProperty}.Length",
        ValidationTargetKind.Array => $"{targetProperty}.Length",
        ValidationTargetKind.Collection => $"{targetProperty}.Count",
        _ => throw new InvalidOperationException($"Unsupported TargetKind for Length rule: {TargetKind}")
    };

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        string lengthAccessor = GetLengthAccessor(targetProperty);

        return $"({targetProperty} is null || ({lengthAccessor} >= {Min} && {lengthAccessor} <= {Max}))";
    }

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string lengthAccessor = GetLengthAccessor(targetProperty);

        string failCondition = $"({targetProperty} is not null && ({lengthAccessor} < {Min} || {lengthAccessor} > {Max}))";

        string defaultMessage = ValidationErrorMessages.Length(propertyName, Min, Max);
        string finalMessage = !string.IsNullOrWhiteSpace(CustomErrorMessage)
            ? CustomErrorMessage!
                .Replace("{PropertyName}", propertyName)
                .Replace("{MinLength}", Min.ToString())
                .Replace("{MaxLength}", Max.ToString())
            : defaultMessage;

        string errorExpression = $"new global::Validated.ValidationError(\"{propertyName}\", \"{finalMessage}\", \"Length\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is LengthRule otherRule &&
            otherRule.Min == Min &&
            otherRule.Max == Max &&
            otherRule.TargetKind == TargetKind &&
            string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

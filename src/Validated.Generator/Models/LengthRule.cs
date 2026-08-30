using System;
using Validated.Generator.Constants;
using Validated.Generator.Enums;
using Validated.Generator.Utilities;

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
            throw new ArgumentException($"Length rule does not support {targetKind}.", nameof(targetKind));
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

        string errorMessageTemplate = GetErrorMessageExpression($"{TypeNames.ValidationErrorMessagesFqn}.LengthRange", CustomErrorMessage!);

        string errorExpression =
            $"new {TypeNames.ValidationErrorFqn}(\"{propertyName}\", " +
            $"{TypeNames.ValidationMessageHelpersFqn}.FormatLength(\"{propertyName}\", {Min}, {Max}, {errorMessageTemplate}), " +
            $"\"Length\")";

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

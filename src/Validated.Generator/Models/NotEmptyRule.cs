using System;
using Validated.Generator.Constants;
using Validated.Generator.Enums;

namespace Validated.Generator.Models;

public sealed class NotEmptyRule : ValidationRule
{
    public ValidationTargetKind TargetKind { get; }
    public string? CustomErrorMessage { get; }

    public NotEmptyRule(ValidationTargetKind targetKind, string? customErrorMessage = null)
    {
        if (targetKind is not (ValidationTargetKind.String or ValidationTargetKind.Array or ValidationTargetKind.Collection or ValidationTargetKind.Enumerable))
        {
            throw new ArgumentException($"NotEmpty rule does not support {targetKind}.", nameof(targetKind));
        }

        TargetKind = targetKind;
        CustomErrorMessage = customErrorMessage;
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return TargetKind switch
        {
            ValidationTargetKind.String => $"({targetProperty} is null || !string.IsNullOrEmpty({targetProperty}))",
            ValidationTargetKind.Array => $"({targetProperty} is null || {targetProperty}.Length > 0)",
            ValidationTargetKind.Collection => $"({targetProperty} is null || {targetProperty}.Count > 0)",
            ValidationTargetKind.Enumerable => $"({targetProperty} is null || global::System.Linq.Enumerable.Any({targetProperty}))",
            _ => throw new InvalidOperationException($"Unsupported TargetKind for NotEmpty rule: {TargetKind}")
        };
    }

    public override (string FailCondition, string ErrorExpression)? BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = TargetKind switch
        {
            ValidationTargetKind.String => $"({targetProperty} is not null && string.IsNullOrEmpty({targetProperty}))",
            ValidationTargetKind.Array => $"({targetProperty} is not null && {targetProperty}.Length == 0)",
            ValidationTargetKind.Collection => $"({targetProperty} is not null && {targetProperty}.Count == 0)",
            ValidationTargetKind.Enumerable => $"({targetProperty} is not null && !global::System.Linq.Enumerable.Any({targetProperty}))",
            _ => throw new InvalidOperationException($"Unsupported TargetKind for NotEmpty rule: {TargetKind}")
        };

        string errorMessageTemplate = GetErrorMessageExpression($"{TypeNames.ValidationErrorMessagesFqn}.NotEmpty", CustomErrorMessage!);

        string errorExpression =
            $"new {TypeNames.ValidationErrorFqn}(\"{propertyName}\", " +
            $"{TypeNames.ValidationMessageHelpersFqn}.Format(\"{propertyName}\", {errorMessageTemplate}), " +
            $"\"NotEmpty\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is NotEmptyRule otherRule &&
            otherRule.TargetKind == TargetKind &&
            string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

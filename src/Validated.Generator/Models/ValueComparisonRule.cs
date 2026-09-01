using System;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Enums;
using Validated.Generator.Utilities;

namespace Validated.Generator.Models;

public sealed class ValueComparisonRule : ValidationRule
{
    public ComparisonOperator Operator { get; }
    public string ValueLiteral { get; }
    public object? Value { get; }
    public string? CustomErrorMessage { get; }

    public ValueComparisonRule(ComparisonOperator op, string valueLiteral, object? value, string? customErrorMessage = null)
    {
        Operator = op;
        ValueLiteral = valueLiteral;
        Value = value;
        CustomErrorMessage = customErrorMessage;
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return $"({TypeNames.ValidationHelpersFqn}.Compare({targetProperty}, {ValueLiteral}, {TypeNames.ComparisonOperatorFqn}.{Operator}))";
    }

    public static string GetComparisonErrorMessage(ComparisonOperator op)
    {
        return op switch
        {
            ComparisonOperator.GreaterThan or
            ComparisonOperator.GreaterThanOrEqual or
            ComparisonOperator.LessThan or
            ComparisonOperator.LessThanOrEqual or
            ComparisonOperator.Equal or
            ComparisonOperator.NotEqual=> $"{TypeNames.ValidationErrorMessagesFqn}.{op}",
            _ => throw new InvalidOperationException($"Unsupported ComparisonOperator for Comparison rule: {op}")
        };
    }

    public override (string FailCondition, string ErrorExpression)? BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"(!{TypeNames.ValidationHelpersFqn}.Compare({targetProperty}, {ValueLiteral}, {TypeNames.ComparisonOperatorFqn}.{Operator}))";

        string errorMessageTemplate = GetErrorMessageExpression(GetComparisonErrorMessage(Operator), CustomErrorMessage!);

        string errorExpression =
            $"new {TypeNames.ValidationErrorFqn}(\"{propertyName}\", " +
            $"{TypeNames.ValidationMessageHelpersFqn}.FormatValueComparison(\"{propertyName}\", {ValueLiteral}, {errorMessageTemplate}), " +
            $"\"ValueComparison\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is ValueComparisonRule otherRule &&
               otherRule.Operator == Operator &&
               string.Equals(otherRule.ValueLiteral, ValueLiteral) &&
               Equals(otherRule.Value, Value) &&
               string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

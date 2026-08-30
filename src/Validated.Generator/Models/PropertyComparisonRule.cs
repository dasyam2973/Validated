using System;
using Validated.Generator.Constants;
using Validated.Generator.Enums;
using Validated.Generator.Utilities;

namespace Validated.Generator.Models;

public sealed class PropertyComparisonRule : ValidationRule
{
    public ComparisonOperator Operator { get; }
    public string OtherProperty { get; }
    public string OtherPropertyName { get; }
    public string? CustomErrorMessage { get; }

    public PropertyComparisonRule(ComparisonOperator op, string otherProperty, string otherPropertyName, string? customErrorMessage = null)
    {
        Operator = op;
        OtherProperty = otherProperty;
        OtherPropertyName = otherPropertyName;
        CustomErrorMessage = customErrorMessage;
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return $"(global::Validated.ValidationHelpers.Compare({targetProperty}, {OtherProperty}, global::Validated.ComparisonOperator.{Operator}))";
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
            ComparisonOperator.NotEqual=> $"{TypeNames.ValidationErrorMessagesFqn}.{op}Property",
            _ => throw new InvalidOperationException($"Unsupported ComparisonOperator for Comparison rule: {op}")
        };
    }

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"(!global::Validated.ValidationHelpers.Compare({targetProperty}, {OtherProperty}, global::Validated.ComparisonOperator.{Operator}))";

        string errorMessageTemplate = GetErrorMessageExpression(GetComparisonErrorMessage(Operator), CustomErrorMessage!);

        string errorExpression =
            $"new {TypeNames.ValidationErrorFqn}(\"{propertyName}\", " +
            $"{TypeNames.ValidationMessageHelpersFqn}.FormatPropertyComparison(\"{propertyName}\", \"{OtherPropertyName}\", {errorMessageTemplate}), " +
            $"\"PropertyComparison\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is PropertyComparisonRule otherRule &&
               otherRule.Operator == Operator &&
               string.Equals(otherRule.OtherProperty, OtherProperty) &&
               string.Equals(otherRule.OtherPropertyName, OtherPropertyName) &&
               string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

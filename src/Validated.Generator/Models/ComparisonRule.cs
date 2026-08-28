using System;
using Validated.Generator.Constants;
using Validated.Generator.Enums;

namespace Validated.Generator.Models;

public sealed class ComparisonRule : ValidationRule
{
    public string OtherProperty { get; }
    public string OtherPropertyName { get; }
    public ComparisonOperator Operator { get; }
    public string? CustomErrorMessage { get; }

    public ComparisonRule(string otherProperty, string otherPropertyName, ComparisonOperator op, string? customErrorMessage = null)
    {
        OtherProperty = otherProperty;
        OtherPropertyName = otherPropertyName;
        Operator = op;
        CustomErrorMessage = customErrorMessage;
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return $"(global::Validated.ValidationHelpers.Compare({targetProperty}, {OtherProperty}, global::Validated.ComparisonOperator.{Operator}))";
    }

    public static string GetComparisonErrorMessage(ComparisonOperator op, string propertyName, string otherPropertyName)
    {
        return op switch
        {
            ComparisonOperator.GreaterThan => ValidationErrorMessages.GreaterThanProperty(propertyName, otherPropertyName),
            ComparisonOperator.GreaterThanOrEqual => ValidationErrorMessages.GreaterThanOrEqualProperty(propertyName, otherPropertyName),
            ComparisonOperator.LessThan => ValidationErrorMessages.LessThanProperty(propertyName, otherPropertyName),
            ComparisonOperator.LessThanOrEqual => ValidationErrorMessages.LessThanOrEqualProperty(propertyName, otherPropertyName),
            ComparisonOperator.Equal => ValidationErrorMessages.EqualProperty(propertyName, otherPropertyName),
            ComparisonOperator.NotEqual => ValidationErrorMessages.NotEqualProperty(propertyName, otherPropertyName),
            _ => throw new ArgumentOutOfRangeException(nameof(op))
        };
    }

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"(!global::Validated.ValidationHelpers.Compare({targetProperty}, {OtherProperty}, global::Validated.ComparisonOperator.{Operator}))";

        string defaultMessage = GetComparisonErrorMessage(Operator, propertyName, OtherPropertyName);
        string finalMessage = !string.IsNullOrWhiteSpace(CustomErrorMessage)
            ? CustomErrorMessage!
                .Replace("{0}", propertyName)
                .Replace("{1}", OtherPropertyName)
            : defaultMessage;

        string errorExpression = $"new global::Validated.ValidationError(\"{propertyName}\", \"{finalMessage}\", \"Comparsion\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is ComparisonRule otherRule &&
            string.Equals(otherRule.OtherProperty, OtherProperty) &&
            string.Equals(otherRule.OtherPropertyName, OtherPropertyName) &&
            otherRule.Operator == Operator &&
            string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

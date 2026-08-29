using System;
using Validated.Generator.Constants;
using Validated.Generator.Enums;
using Validated.Generator.Utilities;

namespace Validated.Generator.Models;

public sealed class ComparisonRule : ValidationRule
{
    public ComparisonOperator Operator { get; }
    public string OtherProperty { get; }
    public string OtherPropertyName { get; }
    public string? CustomErrorMessage { get; }

    public ComparisonRule(ComparisonOperator op, string otherProperty, string otherPropertyName, string? customErrorMessage = null)
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
            ComparisonOperator.GreaterThan => ValidationErrorMessages.GreaterThanProperty,
            ComparisonOperator.GreaterThanOrEqual => ValidationErrorMessages.GreaterThanOrEqualProperty,
            ComparisonOperator.LessThan => ValidationErrorMessages.LessThanProperty,
            ComparisonOperator.LessThanOrEqual => ValidationErrorMessages.LessThanOrEqualProperty,
            ComparisonOperator.Equal => ValidationErrorMessages.EqualProperty,
            ComparisonOperator.NotEqual => ValidationErrorMessages.NotEqualProperty,
            _ => throw new InvalidOperationException($"Unsupported ComparisonOperator for Comparison rule: {op}")
        };
    }

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"(!global::Validated.ValidationHelpers.Compare({targetProperty}, {OtherProperty}, global::Validated.ComparisonOperator.{Operator}))";

        string errorMessage = new MessageFormatter()
            .With(MessageArguments.PropertyName, propertyName)
            .With(MessageArguments.OtherPropertyName, OtherPropertyName)
            .Format(GetComparisonErrorMessage(Operator));

        string errorExpression = $"new global::Validated.ValidationError(\"{propertyName}\", \"{errorMessage}\", \"Comparsion\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is ComparisonRule otherRule &&
               otherRule.Operator == Operator &&
               string.Equals(otherRule.OtherProperty, OtherProperty) &&
               string.Equals(otherRule.OtherPropertyName, OtherPropertyName) &&
               string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

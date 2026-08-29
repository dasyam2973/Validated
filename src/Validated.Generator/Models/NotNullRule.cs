using Validated.Generator.Constants;
using Validated.Generator.Utilities;

namespace Validated.Generator.Models;

public sealed class NotNullRule : ValidationRule
{
    public string? CustomErrorMessage { get; }

    public NotNullRule(string? customErrorMessage = null)
    {
        CustomErrorMessage = customErrorMessage;
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return $"({targetProperty} is not null)";
    }

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"({targetProperty} is null)";

        string errorMessage = new MessageFormatter()
            .With(MessageArguments.PropertyName, propertyName)
            .Format(ValidationErrorMessages.NotNull);

        string errorExpression = $"new global::Validated.ValidationError(\"{propertyName}\", \"{errorMessage}\", \"NotNull\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is NotNullRule otherRule && string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

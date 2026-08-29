using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Utilities;

namespace Validated.Generator.Models;

public sealed class RegexRule : ValidationRule
{
    public string Pattern { get; }
    public string? CustomErrorMessage { get; }

    public RegexRule(string pattern, string? customErrorMessage = null)
    {
        Pattern = pattern;
        CustomErrorMessage = customErrorMessage;
    }

    public override IEnumerable<string> EmitStaticDeclarations(string propertyName)
    {
        string fieldName = GetFieldName(propertyName);

        string escapedPattern = Pattern.Replace("\"", "\"\"");

        yield return $@"private static readonly global::System.Text.RegularExpressions.Regex {fieldName} = new global::System.Text.RegularExpressions.Regex(@""{escapedPattern}"", global::System.Text.RegularExpressions.RegexOptions.Compiled);";
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return $"({targetProperty} is null || {GetFieldName(propertyName)}.IsMatch({targetProperty}))";
    }

    private static string GetFieldName(string propertyName) => $"_regex_{propertyName}";

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"({targetProperty} is not null && !{GetFieldName(propertyName)}.IsMatch({targetProperty}))";

        string errorMessage = new MessageFormatter()
            .With(MessageArguments.PropertyName, propertyName)
            .With(MessageArguments.Pattern, Pattern)
            .Format(ValidationErrorMessages.Regex);

        string errorExpression = $"new global::Validated.ValidationError(\"{propertyName}\", \"{errorMessage}\", \"Regex\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is RegexRule otherRule &&
            string.Equals(otherRule.Pattern, Pattern) &&
            string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

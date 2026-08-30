using System.Collections.Generic;
using Validated.Generator.Constants;

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

    private static string GetFieldName(string propertyName) => $"_regex_{propertyName}";

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

    public override (string FailCondition, string ErrorExpression) BuildErrorCheck(string targetProperty, string propertyName)
    {
        string failCondition = $"({targetProperty} is not null && !{GetFieldName(propertyName)}.IsMatch({targetProperty}))";

        string errorMessageTemplate = GetErrorMessageExpression($"{TypeNames.ValidationErrorMessagesFqn}.Regex", CustomErrorMessage!);

        string errorExpression =
            $"new {TypeNames.ValidationErrorFqn}(\"{propertyName}\", " +
            $"{TypeNames.ValidationMessageHelpersFqn}.Format(\"{propertyName}\", {errorMessageTemplate}), " +
            $"\"Regex\")";

        return (failCondition, errorExpression);
    }

    public override bool Equals(ValidationRule other)
    {
        return other is RegexRule otherRule &&
            string.Equals(otherRule.Pattern, Pattern) &&
            string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

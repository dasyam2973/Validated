using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Models;

namespace Validated.Generator.Parsers;

internal class RegexAttributeParser : IAttributeRuleParser
{
    public string TargetAttributeFullName => TypeNames.VRegexFqn;

    public bool IsApplicableTo(ITypeSymbol typeSymbol, Compilation compilation)
    {
        return typeSymbol.SpecialType == SpecialType.System_String;
    }

    public ValidationRule? Parse(
        AttributeData attribute,
        ISymbol targetProperty,
        ITypeSymbol propertyType,
        Compilation compilation,
        List<Diagnostic> diagnostics)
    {
        var location = attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation() ?? Location.None;

        string? customErrorMessage = null;
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == "ErrorMessage" && namedArg.Value.Value is string msg)
            {
                customErrorMessage = msg;
                break;
            }
        }

        if (attribute.ConstructorArguments.Length == 1 &&
            attribute.ConstructorArguments[0].Value is string pattern)
        {
            return new RegexRule(pattern, customErrorMessage);
        }

        return null;
    }
}

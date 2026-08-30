using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Models;
using Validated.Generator.Utilities;

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
        if (attribute.ConstructorArguments.Length == 1 &&
            attribute.ConstructorArguments[0].Value is string pattern)
        {
            string? customErrorMessage = attribute.GetCustomErrorMessage();

            return new RegexRule(pattern, customErrorMessage);
        }

        return null;
    }
}

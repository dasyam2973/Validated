using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Models;
using Validated.Generator.Utilities;

namespace Validated.Generator.Parsers;

internal class NotNullAttributeParser : IAttributeRuleParser
{
    public string TargetAttributeFullName => TypeNames.VNotNullFqn;

    public bool IsApplicableTo(ITypeSymbol typeSymbol, Compilation compilation)
    {
        return true;
    }

    public ValidationRule? Parse(
        AttributeData attribute,
        ISymbol targetProperty,
        ITypeSymbol propertyType,
        Compilation compilation,
        List<Diagnostic> diagnostics)
    {
        string? customErrorMessage = attribute.GetCustomErrorMessage();

        return new NotNullRule(customErrorMessage);
    }
}

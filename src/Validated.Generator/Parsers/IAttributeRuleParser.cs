using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Models;

namespace Validated.Generator.Parsers;

internal interface IAttributeRuleParser
{
    string TargetAttributeFullName { get; }

    bool IsApplicableTo(ITypeSymbol typeSymbol, Compilation compilation);

    ValidationRule? Parse(
        AttributeData attribute,
        ISymbol targetProperty,
        ITypeSymbol propertyType,
        Compilation compilation,
        List<Diagnostic> diagnostics);
}

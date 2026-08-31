using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Validated.Generator.Constants;
using Validated.Generator.Models;
using Validated.Generator.Utilities;

namespace Validated.Generator.Parsers;

internal class CollectionAttributeParser : IAttributeRuleParser
{
    public string TargetAttributeFullName => TypeNames.ValidateCollectionFqn;

    public bool IsApplicableTo(ITypeSymbol typeSymbol, Compilation compilation)
    {
        ITypeSymbol? elementType = typeSymbol.GetElementType();

        if (elementType != null)
        {
            var iValidatable = compilation.GetTypeByMetadataName("Validated.IValidatable`1");

            var validatableAttr = compilation.GetTypeByMetadataName("Validated.Annotations.ValidatableAttribute");

            bool hasValidatableAttr = validatableAttr != null && elementType.GetAttributes().Any(attr =>
                SymbolEqualityComparer.Default.Equals(attr.AttributeClass, validatableAttr));

            bool isIValidatableInterface = iValidatable != null && (
                SymbolEqualityComparer.Default.Equals(elementType.OriginalDefinition, iValidatable) ||
                elementType.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, iValidatable))
            );

            return hasValidatableAttr || isIValidatableInterface;
        }

        return false;
    }

    public ValidationRule? Parse(
        AttributeData attribute,
        ISymbol targetProperty,
        ITypeSymbol propertyType,
        Compilation compilation,
        List<Diagnostic> diagnostics)
    {
        string? customErrorMessage = attribute.GetCustomErrorMessage();

        return new CollectionRule(customErrorMessage);
    }
}

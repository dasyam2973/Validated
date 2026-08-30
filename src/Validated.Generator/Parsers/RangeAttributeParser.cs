using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Diagnostics;
using Validated.Generator.Models;
using Validated.Generator.Utilities;

namespace Validated.Generator.Parsers;

internal class RangeAttributeParser : IAttributeRuleParser
{
    public string TargetAttributeFullName => TypeNames.VRangeFqn;

    public bool IsApplicableTo(ITypeSymbol typeSymbol, Compilation compilation)
    {
        if (typeSymbol is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } namedType)
        {
            typeSymbol = namedType.TypeArguments[0];
        }

        return typeSymbol.SpecialType is
            SpecialType.System_Int32 or SpecialType.System_Int64 or
            SpecialType.System_Double or SpecialType.System_Single or
            SpecialType.System_Decimal or SpecialType.System_Byte or
            SpecialType.System_Int16;
    }

    public ValidationRule? Parse(
        AttributeData attribute,
        ISymbol targetProperty,
        ITypeSymbol propertyType,
        Compilation compilation,
        List<Diagnostic> diagnostics)
    {
        var location = attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation() ?? Location.None;

        if (attribute.ConstructorArguments.Length == 2 &&
            double.TryParse(attribute.ConstructorArguments[0].Value?.ToString(), out var min) &&
            double.TryParse(attribute.ConstructorArguments[1].Value?.ToString(), out var max))
        {
            if (min > max)
            {
                diagnostics.Add(Diagnostic.Create(
                    DiagnosticDescriptors.InvalidRange,
                    location,
                    min,
                    max
                ));
                return null;
            }

            string? customErrorMessage = attribute.GetCustomErrorMessage();

            return new RangeRule(min, max, customErrorMessage);
        }

        return null;
    }
}

using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Diagnostics;
using Validated.Generator.Enums;
using Validated.Generator.Extensions;
using Validated.Generator.Models;

namespace Validated.Generator.Parsers;

internal class LengthAttributeParser : IAttributeRuleParser
{
    public string TargetAttributeFullName => TypeNames.VLengthFqn;

    public bool IsApplicableTo(ITypeSymbol typeSymbol, Compilation compilation)
    {
        var targetKind = typeSymbol.GetValidationTargetKind(compilation);
        return targetKind == ValidationTargetKind.String ||
               targetKind == ValidationTargetKind.Array ||
               targetKind == ValidationTargetKind.Collection;
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

        if (attribute.ConstructorArguments.Length == 2 &&
            int.TryParse(attribute.ConstructorArguments[0].Value?.ToString(), out var min) &&
            int.TryParse(attribute.ConstructorArguments[1].Value?.ToString(), out var max))
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

            var targetKind = propertyType.GetValidationTargetKind(compilation);
            return new LengthRule(targetKind, min, max, customErrorMessage);
        }

        return null;
    }
}

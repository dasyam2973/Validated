using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Diagnostics;
using Validated.Generator.Models;

namespace Validated.Generator.Parsers;

internal class StringLengthAttributeParser : IAttributeRuleParser
{
    public string TargetAttributeFullName => TypeNames.VStringLengthFqn;

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

            return new StringLengthRule(min, max, customErrorMessage);
        }

        return null;
    }
}

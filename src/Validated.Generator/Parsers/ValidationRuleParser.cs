using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Validated.Generator.Diagnostics;
using Validated.Generator.Models;

namespace Validated.Generator.Parsers;

public static class ValidationRuleParser
{
    private static readonly Dictionary<string, IAttributeRuleParser> Parsers = new IAttributeRuleParser[]
    {
        new NotNullAttributeParser(),
        new NotEmptyAttributeParser(),
        new RangeAttributeParser(),
        new StringLengthAttributeParser(),
        new LengthAttributeParser(),

        new RegexAttributeParser(),

        new GreaterThanOrEqualAttributeParser(),
        new GreaterThanAttributeParser(),
        new EqualAttributeParser()
    }.ToDictionary(p => p.TargetAttributeFullName, p => p);

    public static ValidationRule? ParseAttribute(
        AttributeData attribute,
        ISymbol targetProperty,
        ITypeSymbol propertyType,
        Compilation compilation,
        List<Diagnostic> diagnostics)
    {
        var attrClass = attribute.AttributeClass;
        if (attrClass is null) return null;

        string attrFullName = attrClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                       .Replace("global::", "");

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

        if (Parsers.TryGetValue(attrFullName, out var parser))
        {
            if (!parser.IsApplicableTo(propertyType, compilation))
            {
                diagnostics.Add(Diagnostic.Create(
                    DiagnosticDescriptors.InvalidAttributeTarget,
                    location,
                    attrFullName, propertyType.ToDisplayString()
                ));
                return null;
            }

            return parser.Parse(attribute, targetProperty, propertyType, compilation, diagnostics);
        }

        return null;
    }
}

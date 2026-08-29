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
        new LengthAttributeParser(),

        new RegexAttributeParser(),

        new ValueComparisonAttributeParser.GreaterThan(),

        new PropertyComparisonAttributeParser.GreaterThan(),
        new PropertyComparisonAttributeParser.GreaterThanOrEqual(),
        new PropertyComparisonAttributeParser.LessThan(),
        new PropertyComparisonAttributeParser.Equal()
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

        string rawFullName = attrClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .Replace("global::", "");

        string cleanFullName = CutGenericSuffix(rawFullName);

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

        if (Parsers.TryGetValue(cleanFullName, out var parser))
        {
            if (!parser.IsApplicableTo(propertyType, compilation))
            {
                diagnostics.Add(Diagnostic.Create(
                    DiagnosticDescriptors.InvalidAttributeTarget,
                    location,
                    cleanFullName, propertyType.ToDisplayString()
                ));
                return null;
            }

            return parser.Parse(attribute, targetProperty, propertyType, compilation, diagnostics);
        }

        return null;
    }

    private static string CutGenericSuffix(string fullName)
    {
        int genericIdx = fullName.IndexOf('<');
        return genericIdx >= 0 ? fullName.Substring(0, genericIdx) : fullName;
    }
}

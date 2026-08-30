using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Diagnostics;
using Validated.Generator.Enums;
using Validated.Generator.Models;
using Validated.Generator.Utilities;

namespace Validated.Generator.Parsers;

internal abstract partial class ValueComparisonAttributeParser : IAttributeRuleParser
{
    public abstract string TargetAttributeFullName { get; }

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
        var attrClass = attribute.AttributeClass;
        if (attrClass is null) return null;

        string rawFullName = attrClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
                                      .Replace("global::", "");

        string cleanFullName = CutGenericSuffix(rawFullName);

        var location = attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation() ?? Location.None;

        if (attribute.ConstructorArguments.Length == 1)
        {
            TypedConstant typedConstant = attribute.ConstructorArguments[0];

            if (typedConstant.Type is not null && !propertyType.IsComparableWith(typedConstant.Type, compilation))
            {
                diagnostics.Add(Diagnostic.Create(
                    DiagnosticDescriptors.IncompatibleCompareTypes,
                    location,
                    propertyType.ToDisplayString(), targetProperty.Name,
                    typedConstant.Type.ToDisplayString(), "Value"
                ));
                return null;
            }

            string valueLiteral = ToCSharpValueLiteral(typedConstant);

            var compOp = cleanFullName switch
            {
                TypeNames.VGreaterThanFqn => ComparisonOperator.GreaterThan,
                TypeNames.VGreaterThanOrEqualFqn => ComparisonOperator.GreaterThanOrEqual,
                TypeNames.VLessThanFqn => ComparisonOperator.LessThan,
                TypeNames.VLessThanOrEqualFqn => ComparisonOperator.LessThanOrEqual,
                TypeNames.VEqualFqn => ComparisonOperator.Equal,
                TypeNames.VNotEqualFqn => ComparisonOperator.NotEqual,
                _ => ComparisonOperator.None
            };

            string? customErrorMessage = attribute.GetCustomErrorMessage();

            if (compOp != ComparisonOperator.None)
            {
                return new ValueComparisonRule(compOp, valueLiteral, typedConstant.Value, customErrorMessage);
            }
        }

        return null;
    }

    private static string ToCSharpValueLiteral(TypedConstant constant)
    {
        if (constant.IsNull) return "null";

        if (constant.Kind == TypedConstantKind.Enum && constant.Type is not null)
        {
            var enumTypeName = constant.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            return $"({enumTypeName}){constant.Value}";
        }

        return SymbolDisplay.FormatPrimitive(constant.Value!, quoteStrings: true, useHexadecimalNumbers: false);
    }

    private static string CutGenericSuffix(string fullName)
    {
        int genericIdx = fullName.IndexOf('<');
        return genericIdx >= 0 ? fullName.Substring(0, genericIdx) : fullName;
    }
}

using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Validated.Generator.Constants;
using Validated.Generator.Diagnostics;
using Validated.Generator.Enums;
using Validated.Generator.Models;
using Validated.Generator.Utilities;

namespace Validated.Generator.Parsers;

internal abstract class ComparisonAttributeParser : IAttributeRuleParser
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

        if (attribute.ConstructorArguments.Length == 1 &&
            attribute.ConstructorArguments[0].Value is string otherPropertyName)
        {
            INamedTypeSymbol containingType = targetProperty.ContainingType;
            ISymbol? otherMember = containingType.GetMembers(otherPropertyName).FirstOrDefault();

            ITypeSymbol? otherType = otherMember switch
            {
                IPropertySymbol p => p.Type,
                IFieldSymbol f => f.Type,
                _ => null
            };

            if (otherType is null)
            {
                diagnostics.Add(Diagnostic.Create(
                    DiagnosticDescriptors.MemberNotFound,
                    attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                    otherPropertyName, containingType.Name
                ));
                return null;
            }

            if (!propertyType.IsComparableWith(otherType, compilation))
            {
                diagnostics.Add(Diagnostic.Create(
                    DiagnosticDescriptors.IncompatibleCompareTypes,
                    attribute.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                    propertyType.ToDisplayString(), targetProperty.Name,
                    otherType.ToDisplayString(), otherPropertyName
                ));
                return null;
            }

            var compOp = attrFullName switch
            {
                TypeNames.VGreaterThanOrEqualFqn => ComparisonOperator.GreaterThanOrEqual,
                TypeNames.VGreaterThanFqn => ComparisonOperator.GreaterThan,
                TypeNames.VEqualFqn => ComparisonOperator.Equal,
                _ => ComparisonOperator.None
            };

            if (compOp != ComparisonOperator.None)
            {
                return new ComparisonRule(compOp, $"this.{otherPropertyName}", otherPropertyName, customErrorMessage);
            }
        }

        return null;
    }
}

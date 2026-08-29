using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Enums;
using Validated.Generator.Models;
using Validated.Generator.Utilities;

namespace Validated.Generator.Parsers;

internal class NotEmptyAttributeParser : IAttributeRuleParser
{
    public string TargetAttributeFullName => TypeNames.VNotEmptyFqn;

    public bool IsApplicableTo(ITypeSymbol typeSymbol, Compilation compilation)
    {
        var targetKind = typeSymbol.GetValidationTargetKind(compilation);
        return targetKind == ValidationTargetKind.String ||
               targetKind == ValidationTargetKind.Array ||
               targetKind == ValidationTargetKind.Collection ||
               targetKind == ValidationTargetKind.Enumerable;
    }

    public ValidationRule? Parse(
        AttributeData attribute,
        ISymbol targetProperty,
        ITypeSymbol propertyType,
        Compilation compilation,
        List<Diagnostic> diagnostics)
    {
        string? customErrorMessage = null;
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == "ErrorMessage" && namedArg.Value.Value is string msg)
            {
                customErrorMessage = msg;
                break;
            }
        }

        var targetKind = propertyType.GetValidationTargetKind(compilation);
        return new NotEmptyRule(targetKind, customErrorMessage);
    }
}

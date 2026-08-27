using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Validated.Generator.Constants;
using Validated.Generator.Models;

namespace Validated.Generator.Parsers;

internal class NotNullAttributeParser : IAttributeRuleParser
{
    public string TargetAttributeFullName => TypeNames.VNotNullFqn;

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

        return new NotNullRule(customErrorMessage);
    }
}

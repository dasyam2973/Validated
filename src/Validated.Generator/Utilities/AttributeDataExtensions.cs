using Microsoft.CodeAnalysis;

namespace Validated.Generator.Utilities;

public static class AttributeDataExtensions
{
    public static string? GetCustomErrorMessage(this AttributeData attribute)
    {
        foreach (var namedArg in attribute.NamedArguments)
        {
            if (namedArg.Key == "ErrorMessage" && namedArg.Value.Value is string msg)
            {
                return msg;
            }
        }
        return null;
    }
}

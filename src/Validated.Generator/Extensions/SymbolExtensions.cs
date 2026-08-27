using Microsoft.CodeAnalysis;
using System.Linq;

namespace Validated.Generator.Extensions;

internal static class SymbolExtensions
{
    public static bool IsComparableWith(this ITypeSymbol typeA, ITypeSymbol typeB, Compilation compilation)
    {
        var unwrapA = typeA;
        if (typeA is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } namedTypeA)
            unwrapA = namedTypeA.TypeArguments[0];

        var unwrapB = typeB;
        if (typeB is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } namedTypeB)
            unwrapB = namedTypeB.TypeArguments[0];

        if (!SymbolEqualityComparer.Default.Equals(unwrapA, unwrapB))
            return false;

        INamedTypeSymbol? genericIComparableSymbol = compilation.GetTypeByMetadataName("System.IComparable`1");

        return unwrapA.SpecialType switch
        {
            SpecialType.System_String => true,
            SpecialType.System_DateTime => true,
            _ when unwrapA.SpecialType != SpecialType.None => true,
            _ => unwrapA.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i.OriginalDefinition, genericIComparableSymbol))
        };
    }

    public static string GetTypeDeclarationKeyword(this INamedTypeSymbol typeSymbol)
    {
        if (typeSymbol.IsRecord)
        {
            return typeSymbol.IsValueType ? "record struct" : "record";
        }

        return typeSymbol.TypeKind switch
        {
            TypeKind.Class => "class",
            TypeKind.Struct => "struct",
            TypeKind.Interface => "interface",
            _ => "class"
        };
    }
}

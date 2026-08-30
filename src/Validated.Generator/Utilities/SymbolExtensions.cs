using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;
using Validated.Generator.Enums;

namespace Validated.Generator.Utilities;

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

        if (unwrapA.TypeKind == TypeKind.Enum)
            return true;

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

    public static bool IsCollection(this ITypeSymbol typeSymbol, Compilation compilation)
    {
        var iCollection = compilation.GetTypeByMetadataName("System.Collections.ICollection");
        var iCollectionGeneric = compilation.GetTypeByMetadataName("System.Collections.Generic.ICollection`1");

        if (iCollection != null &&
            (SymbolEqualityComparer.Default.Equals(typeSymbol, iCollection) ||
             typeSymbol.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, iCollection))))
        {
            return true;
        }

        if (iCollectionGeneric != null)
        {
            if (typeSymbol is INamedTypeSymbol named && named.IsGenericType &&
                SymbolEqualityComparer.Default.Equals(named.ConstructedFrom, iCollectionGeneric))
            {
                return true;
            }

            if (typeSymbol.AllInterfaces.Any(i => i.IsGenericType &&
                SymbolEqualityComparer.Default.Equals(i.ConstructedFrom, iCollectionGeneric)))
            {
                return true;
            }
        }

        return false;
    }

    public static ValidationTargetKind GetValidationTargetKind(this ITypeSymbol typeSymbol, Compilation compilation)
    {
        // string
        if (typeSymbol.SpecialType == SpecialType.System_String)
            return ValidationTargetKind.String;

        // Array
        if (typeSymbol.TypeKind == TypeKind.Array)
            return ValidationTargetKind.Array;

        // ICollection, ICollection<T>
        if (typeSymbol.IsCollection(compilation))
            return ValidationTargetKind.Collection;

        // IEnumerable, IEnumerable<T>
        var iEnumerable = compilation.GetTypeByMetadataName("System.Collections.IEnumerable");

        bool isEnumerable = iEnumerable != null &&
            (SymbolEqualityComparer.Default.Equals(typeSymbol, iEnumerable) ||
             typeSymbol.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, iEnumerable)));

        if (isEnumerable)
            return ValidationTargetKind.Enumerable;

        return ValidationTargetKind.None;
    }
}

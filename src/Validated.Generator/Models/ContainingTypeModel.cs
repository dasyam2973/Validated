using System;

namespace Validated.Generator.Models;

public sealed class ContainingTypeModel : IEquatable<ContainingTypeModel>
{
    public string TypeName { get; }
    public string DeclarationKeyword { get; }

    public ContainingTypeModel(string typeName, string declarationKeyword)
    {
        TypeName = typeName;
        DeclarationKeyword = declarationKeyword;
    }

    public bool Equals(ContainingTypeModel? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return TypeName == other.TypeName && DeclarationKeyword == other.DeclarationKeyword;
    }

    public override bool Equals(object obj)
    {
        return obj is ContainingTypeModel other && Equals(other);
    }

    public override int GetHashCode() => (TypeName, DeclarationKeyword).GetHashCode();
}

using System;

namespace Validated.Generator.Models;

public sealed class ValidatableTypeModel : IEquatable<ValidatableTypeModel>
{
    public string Namespace { get; }
    public string TypeName { get; }
    public string DeclarationKeyword { get; }
    public EquatableArray<ValidatablePropertyModel> Properties { get; }

    public ValidatableTypeModel(string namespaceName, string typeName, string declarationKeyword, EquatableArray<ValidatablePropertyModel> properties)
    {
        Namespace = namespaceName;
        TypeName = typeName;
        DeclarationKeyword = declarationKeyword;
        Properties = properties;
    }

    public bool Equals(ValidatableTypeModel other)
    {
        return Namespace.Equals(other.Namespace) &&
            TypeName.Equals(other.TypeName) &&
            DeclarationKeyword.Equals(other.DeclarationKeyword) &&
            Properties.Equals(other.Properties);
    }

    public override bool Equals(object obj)
    {
        return obj is ValidatableTypeModel other && Equals(other);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Namespace);
        hashCode.Add(TypeName);
        hashCode.Add(DeclarationKeyword);
        foreach (var item in Properties)
        {
            hashCode.Add(item);
        }
        return hashCode.ToHashCode();
    }
}

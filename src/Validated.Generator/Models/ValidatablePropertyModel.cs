using System;
using Validated.Generator.Utilities;

namespace Validated.Generator.Models;

public sealed class ValidatablePropertyModel : IEquatable<ValidatablePropertyModel>
{
    public string Name { get; }
    public string TypeName { get; }
    public EquatableArray<ValidationRule> Rules { get; }

    public ValidatablePropertyModel(string name, string typeName, EquatableArray<ValidationRule> rules)
    {
        Name = name;
        TypeName = typeName;
        Rules = rules;
    }

    public bool Equals(ValidatablePropertyModel other)
    {
        return Name.Equals(other.Name) && TypeName.Equals(other.TypeName) && Rules.Equals(other.Rules);
    }

    public override bool Equals(object obj)
    {
        return obj is ValidatablePropertyModel other && Equals(other);
    }

    public override int GetHashCode()
    {
        HashCode hashCode = new();
        hashCode.Add(Name);
        hashCode.Add(TypeName);
        foreach (var item in Rules)
        {
            hashCode.Add(item);
        }
        return hashCode.ToHashCode();
    }
}

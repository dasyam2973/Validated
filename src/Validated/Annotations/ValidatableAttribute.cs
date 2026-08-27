using System;

namespace Validated.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class ValidatableAttribute : Attribute
{
}

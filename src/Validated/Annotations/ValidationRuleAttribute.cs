using System;

namespace Validated.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public abstract class ValidationRuleAttribute : Attribute
{
    public string? ErrorMessage { get; set; }
}
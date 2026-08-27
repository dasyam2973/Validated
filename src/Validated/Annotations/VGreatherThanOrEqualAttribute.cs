using System;

namespace Validated.Annotations;

public sealed class VGreaterThanOrEqualAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VGreaterThanOrEqualAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

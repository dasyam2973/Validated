namespace Validated.Annotations;

public sealed class VGreaterThanOrEqualPropertyAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VGreaterThanOrEqualPropertyAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

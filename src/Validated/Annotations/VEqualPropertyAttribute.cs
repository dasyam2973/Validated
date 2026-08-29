namespace Validated.Annotations;

public sealed class VEqualPropertyAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VEqualPropertyAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

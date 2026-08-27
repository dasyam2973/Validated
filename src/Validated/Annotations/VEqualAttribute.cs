namespace Validated.Annotations;

public sealed class VEqualAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VEqualAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

namespace Validated.Annotations;

public sealed class VGreaterThanAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VGreaterThanAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

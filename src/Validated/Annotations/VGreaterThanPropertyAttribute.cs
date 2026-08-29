namespace Validated.Annotations;

public sealed class VGreaterThanPropertyAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VGreaterThanPropertyAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

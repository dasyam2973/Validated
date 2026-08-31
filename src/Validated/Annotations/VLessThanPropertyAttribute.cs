namespace Validated.Annotations;

public sealed class VLessThanPropertyAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VLessThanPropertyAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

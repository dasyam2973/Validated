namespace Validated.Annotations;

public sealed class VLessThanOrEqualPropertyAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VLessThanOrEqualPropertyAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

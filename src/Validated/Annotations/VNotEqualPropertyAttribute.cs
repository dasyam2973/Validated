namespace Validated.Annotations;

public sealed class VNotEqualPropertyAttribute : ValidationRuleAttribute
{
    public string OtherPropertyName { get; }

    public VNotEqualPropertyAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
    }
}

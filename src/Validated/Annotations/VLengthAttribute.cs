namespace Validated.Annotations;

public sealed class VLengthAttribute : ValidationRuleAttribute
{
    public int Min { get; }
    public int Max { get; }

    public VLengthAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}

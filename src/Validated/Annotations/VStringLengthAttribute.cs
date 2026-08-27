namespace Validated.Annotations;

public sealed class VStringLengthAttribute : ValidationRuleAttribute
{
    public int Min { get; }
    public int Max { get; }

    public VStringLengthAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }
}

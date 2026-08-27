namespace Validated.Annotations;

public sealed class VRangeAttribute : ValidationRuleAttribute
{
    public double Min { get; }
    public double Max { get; }

    public VRangeAttribute(double min, double max)
    {
        Min = min;
        Max = max;
    }
}

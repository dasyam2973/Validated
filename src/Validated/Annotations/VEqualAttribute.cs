namespace Validated.Annotations;

public sealed class VEqualAttribute<T> : ValidationRuleAttribute
{
    public T Value { get; }

    public VEqualAttribute(T value)
    {
        Value = value;
    }
}

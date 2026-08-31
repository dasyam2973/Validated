namespace Validated.Annotations;

public sealed class VNotEqualAttribute<T> : ValidationRuleAttribute
{
    public T Value { get; }

    public VNotEqualAttribute(T value)
    {
        Value = value;
    }
}

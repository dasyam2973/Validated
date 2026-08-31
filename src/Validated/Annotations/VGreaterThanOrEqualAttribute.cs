namespace Validated.Annotations;

public sealed class VGreaterThanOrEqualAttribute<T> : ValidationRuleAttribute
{
    public T Value { get; }

    public VGreaterThanOrEqualAttribute(T value)
    {
        Value = value;
    }
}

namespace Validated.Annotations;

public sealed class VGreaterThanAttribute<T> : ValidationRuleAttribute
{
    public T Value { get; }

    public VGreaterThanAttribute(T value)
    {
        Value = value;
    }
}

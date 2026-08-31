namespace Validated.Annotations;

public sealed class VLessThanAttribute<T> : ValidationRuleAttribute
{
    public T Value { get; }

    public VLessThanAttribute(T value)
    {
        Value = value;
    }
}

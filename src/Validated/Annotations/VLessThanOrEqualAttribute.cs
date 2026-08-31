namespace Validated.Annotations;

public sealed class VLessThanOrEqualAttribute<T> : ValidationRuleAttribute
{
    public T Value { get; }

    public VLessThanOrEqualAttribute(T value)
    {
        Value = value;
    }
}

namespace Validated;

public interface IValidatable<T>
{
    public bool IsValid { get; }
    public ValidationResult Validate();
    public bool TryValidate(out ValidationError error);
    public bool TryValidateProperty(string propertyName, out ValidationError error);
}

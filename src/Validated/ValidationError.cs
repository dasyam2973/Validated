namespace Validated;

public readonly struct ValidationError
{
    public string PropertyName { get; }
    public string Message { get; }
    public string RuleName { get; }

    public ValidationError(string propertyName, string message, string ruleName)
    {
        PropertyName = propertyName;
        Message = message;
        RuleName = ruleName;
    }
}

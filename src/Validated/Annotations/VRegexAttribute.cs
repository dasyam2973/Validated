namespace Validated.Annotations;

public sealed class VRegexAttribute : ValidationRuleAttribute
{
    public string Pattern { get; }

    public VRegexAttribute(string pattern)
    {
        Pattern = pattern;
    }
}

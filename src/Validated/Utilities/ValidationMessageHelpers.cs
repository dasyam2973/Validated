using Validated.Constants;

namespace Validated.Utilities;

public static class ValidationMessageHelpers
{
    public static string Format(string propertyName, string template)
    {
        return new MessageFormatter()
            .WithRaw(MessageArguments.PropertyName, propertyName)
            .Format(template);
    }

    public static string FormatLength(string propertyName, int min, int max, string template)
    {
        return new MessageFormatter()
            .WithRaw(MessageArguments.PropertyName, propertyName)
            .With(MessageArguments.Min, min)
            .With(MessageArguments.Max, max)
            .Format(template);
    }

    public static string FormatRange(string propertyName, object min, object max, string template)
    {
        return new MessageFormatter()
            .WithRaw(MessageArguments.PropertyName, propertyName)
            .With(MessageArguments.Min, min)
            .With(MessageArguments.Max, max)
            .Format(template);
    }

    public static string FormatValueComparison(string propertyName, object value, string template)
    {
        return new MessageFormatter()
            .WithRaw(MessageArguments.PropertyName, propertyName)
            .With(MessageArguments.Value, value)
            .Format(template);
    }

    public static string FormatPropertyComparison(string propertyName, string otherPropertyName, string template)
    {
        return new MessageFormatter()
            .WithRaw(MessageArguments.PropertyName, propertyName)
            .WithRaw(MessageArguments.OtherPropertyName, otherPropertyName)
            .Format(template);
    }
}

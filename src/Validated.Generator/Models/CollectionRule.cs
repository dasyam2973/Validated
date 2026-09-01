using Validated.Generator.Constants;
using Validated.Generator.Utilities;

namespace Validated.Generator.Models;

public sealed class CollectionRule : ValidationRule
{
    public string? CustomErrorMessage { get; }

    public CollectionRule(string? customErrorMessage = null)
    {
        CustomErrorMessage = customErrorMessage;
    }

    public override string BuildCondition(string targetProperty, string propertyName)
    {
        return $"({TypeNames.ValidationHelpersFqn}.IsCollectionValid({targetProperty}, static x => x.IsValid))";
    }

    public override void EmitValidateCode(IndentedStringBuilder builder, string targetProperty, string propertyName)
    {
        builder.Line($"{TypeNames.ValidationHelpersFqn}.ValidateCollection({targetProperty}, \"{propertyName}\", errors, static x => x.Validate());");
    }

    public override void EmitTryValidateCode(IndentedStringBuilder builder, string targetProperty, string propertyName)
    {
        using (builder.Block($"if (!{TypeNames.ValidationHelpersFqn}.TryValidateCollection({targetProperty}, \"{propertyName}\", out error, static x => x.Validate()))"))
        {
            builder.Line("return false;");
        }
    }

    public override bool Equals(ValidationRule other)
    {
        return other is CollectionRule otherRule && string.Equals(otherRule.CustomErrorMessage, CustomErrorMessage);
    }
}

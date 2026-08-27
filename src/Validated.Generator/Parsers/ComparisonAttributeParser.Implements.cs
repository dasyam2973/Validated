using Validated.Generator.Constants;

namespace Validated.Generator.Parsers;

internal sealed class GreaterThanOrEqualAttributeParser : ComparisonAttributeParser
{
    public override string TargetAttributeFullName => TypeNames.VGreaterThanOrEqualFqn;
}

internal sealed class GreaterThanAttributeParser : ComparisonAttributeParser
{
    public override string TargetAttributeFullName => TypeNames.VGreaterThanFqn;
}

internal sealed class EqualAttributeParser : ComparisonAttributeParser
{
    public override string TargetAttributeFullName => TypeNames.VEqualFqn;
}
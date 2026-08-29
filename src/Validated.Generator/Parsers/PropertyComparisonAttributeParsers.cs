using Validated.Generator.Constants;

namespace Validated.Generator.Parsers;

partial class PropertyComparisonAttributeParser
{
    internal sealed class GreaterThan : PropertyComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VGreaterThanPropertyFqn;
    }

    internal sealed class GreaterThanOrEqual : PropertyComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VGreaterThanOrEqualPropertyFqn;
    }

    internal sealed class LessThan : PropertyComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VLessThanPropertyFqn;
    }

    internal sealed class Equal : PropertyComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VEqualPropertyFqn;
    }
}
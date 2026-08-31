using Validated.Generator.Constants;

namespace Validated.Generator.Parsers;

partial class ValueComparisonAttributeParser
{
    internal sealed class GreaterThan : ValueComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VGreaterThanFqn;
    }

    internal sealed class GreaterThanOrEqual : ValueComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VGreaterThanOrEqualFqn;
    }

    internal sealed class LessThan : ValueComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VLessThanFqn;
    }

    internal sealed class LessThanOrEqual : ValueComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VLessThanOrEqualFqn;
    }

    internal sealed class Equal : ValueComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VEqualFqn;
    }

    internal sealed class NotEqual : ValueComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VNotEqualFqn;
    }
}
using Validated.Generator.Constants;

namespace Validated.Generator.Parsers;

partial class ValueComparisonAttributeParser
{
    internal sealed class GreaterThan : ValueComparisonAttributeParser
    {
        public override string TargetAttributeFullName => TypeNames.VGreaterThanFqn;
    }
}
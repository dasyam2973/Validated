using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Validated;

public static class ValidationHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Compare<T>(T left, T right, ComparisonOperator op)
    {
        if (op == ComparisonOperator.Equal)
            return EqualityComparer<T>.Default.Equals(left, right);

        if (op == ComparisonOperator.NotEqual)
            return !EqualityComparer<T>.Default.Equals(left, right);

        int result = Comparer<T>.Default.Compare(left, right);
        return op switch
        {
            ComparisonOperator.GreaterThan => result > 0,
            ComparisonOperator.GreaterThanOrEqual => result >= 0,
            ComparisonOperator.LessThan => result < 0,
            ComparisonOperator.LessThanOrEqual => result <= 0,
            _ => true
        };
    }
}

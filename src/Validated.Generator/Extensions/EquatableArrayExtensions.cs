using System;
using System.Collections.Generic;

namespace Validated.Generator.Extensions;

internal static class EquatableArrayExtensions
{
    internal static EquatableArray<T> ToEquatableArray<T>(this IEnumerable<T> source) where T : IEquatable<T>
    {
        return new EquatableArray<T>(source);
    }
}

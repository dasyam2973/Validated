using System.Collections.Generic;

namespace Validated;

public readonly struct ValidationResult
{
    private static readonly ValidationError[] EmptyErrors = [];

    public bool IsValid => Errors.Count == 0;
    public IReadOnlyList<ValidationError> Errors { get; }

    public ValidationResult(IReadOnlyList<ValidationError>? errors)
    {
        Errors = errors ?? EmptyErrors;
    }

    public static ValidationResult Success => new(EmptyErrors);
}

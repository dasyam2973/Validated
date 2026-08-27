using Microsoft.CodeAnalysis;

namespace Validated.Generator.Diagnostics;

internal static class DiagnosticDescriptors
{
    private const string Category = "ValidationUsage";

    public static readonly DiagnosticDescriptor InvalidRange = new(
        id: "VD001",
        title: "Invalid range configuration",
        messageFormat: "Min ({0}) cannot be greater than Max ({1}). The validation logic may not work correctly.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MemberNotFound = new(
        id: "VD002",
        title: "Target member not found",
        messageFormat: "Member '{0}' was not found on target type",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor IncompatibleCompareTypes = new(
        id: "VD003",
        title: "Incompatible comparison types",
        messageFormat: "Member '{1}' ({0}) cannot be compared with member '{3}' ({2})",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor InvalidAttributeTarget = new(
        id: "VD004",
        title: "Invalid attribute target type",
        messageFormat: "Attribute '{0}' cannot be applied to type '{1}'. Target must be a supported type.",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}

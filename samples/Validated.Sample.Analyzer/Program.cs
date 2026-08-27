using Validated.Annotations;

namespace Validated.Sample.Analyzer;

[Validatable]
public partial class InvalidUsageSample
{
    // [VD001]
    [VRange(50, 10)]
    public int Age { get; set; }

    // [VD002]
    [VGreaterThanOrEqual("NonExistingProperty")]
    public DateTime EndDate { get; set; }

    // [VD003]
    public int MinValue { get; set; }

    [VGreaterThan(nameof(MinValue))]
    public string Description { get; set; } = string.Empty;
}

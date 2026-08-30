using Validated.Annotations;

namespace Validated.Sample.Analyzer;

[Validatable]
public partial class InvalidUsageSample
{
    // [VD001]
    [VRange(50, 10)]
    public int Age { get; set; }

    // [VD002]
    [VGreaterThanOrEqualProperty("NonExistingProperty")]
    public DateTime EndDate { get; set; }

    // [VD003]
    public int MinValue { get; set; }

    [VGreaterThanProperty(nameof(MinValue))]
    public string Description { get; set; } = string.Empty;

    // [VD004]
    [VRegex(@"^.+$")]
    public int NotInt { get; set; }
}

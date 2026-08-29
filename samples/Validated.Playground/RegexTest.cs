using Validated.Annotations;

namespace Validated.Playground;

public partial class RegexTest
{
    [Validatable]
    partial record RegexTestRecord(
        [property: VRegex(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?(\?.*)?$")] string Url
    );

    public static void Run()
    {
        Console.WriteLine($"===== RegexTest =====");

        RegexTestRecord instanceValid = new(Url: "https://google.com");
        var validResult = instanceValid.Validate();

        Console.WriteLine($"IsValid: {validResult.IsValid}");

        RegexTestRecord instanceInvalid = new(Url: "httpt://invalid-scheme.com");
        var invalidResult = instanceInvalid.Validate();

        Console.WriteLine($"IsValid: {invalidResult.IsValid}");
        if (invalidResult.Errors.Count > 0)
        {
            Console.WriteLine("\n[Validation Failures]");
            foreach (var error in invalidResult.Errors)
            {
                Console.WriteLine($"- [{error.PropertyName}] {error.Message}");
            }
        }
    }
}

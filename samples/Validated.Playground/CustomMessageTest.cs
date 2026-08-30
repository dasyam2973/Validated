using Validated.Annotations;

namespace Validated.Playground;

public partial class CustomMessageTest
{
    [Validatable]
    partial record CustomMessageTestRecord(
        [property: VGreaterThan<int>(10, ErrorMessage = "아 Int!!")] int Int,

        [property: VGreaterThanProperty("Int")] int BigInt,

        [property: VNotNull(ErrorMessage = "\"String\"이 문제예요")] string? String
    );

    public static void Run()
    {
        CustomMessageTestRecord instance = new(
            Int: 9,
            BigInt: 20,
            String: null
        );

        var result = instance.Validate();

        Console.WriteLine($"===== CustomMessageTest =====");
        Console.WriteLine($"IsValid: {result.IsValid}");

        if (result.Errors.Count > 0)
        {
            Console.WriteLine("\n[Validation Failures]");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- [{error.PropertyName}] {error.Message}");
            }
        }
    }
}

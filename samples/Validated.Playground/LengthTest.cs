using System.Collections;
using Validated.Annotations;

namespace Validated.Playground;

public partial class LengthTest
{
    [Validatable]
    partial record LengthTestRecord(
        [property: VLength(1, 10)] string? String,
        [property: VLength(1, 10)] int[]? Array,
        [property: VLength(1, 10)] List<int>? List,
        [property: VLength(1, 10)] ArrayList? ArrayList
    );

    public static void Run()
    {
        LengthTestRecord instance = new(
            String: "01234567890",
            Array: [0],
            List: [],
            ArrayList: null // null은 스킵
        );

        var result = instance.Validate();

        Console.WriteLine($"===== LengthTest =====");
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

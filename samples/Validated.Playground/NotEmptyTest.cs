using System.Collections;
using Validated.Annotations;

namespace Validated.Playground;

public partial class NotEmptyTest
{
    [Validatable]
    partial record NotEmptyTestRecord(
        [property: VNotEmpty] string? String,
        [property: VNotEmpty] int[]? Array,
        [property: VNotEmpty] List<int>? List,
        [property: VNotEmpty] ArrayList? ArrayList,
        [property: VNotEmpty] IEnumerable<int>? Enumerable
    );

    public static void Run()
    {
        NotEmptyTestRecord instance = new(
            String: "",
            Array: [1, 2, 3, 4, 5],
            List: [],
            ArrayList: [1, 'A', "Banana"],
            Enumerable: null // null은 스킵
        );

        var result = instance.Validate();

        Console.WriteLine($"===== NotEmptyTest =====");
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

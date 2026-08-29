using System.Collections;
using Validated.Annotations;

namespace Validated.Playground;

internal partial class Program
{
    [Validatable]
    partial struct NotEmptyTest
    {
        [VNotEmpty] public string? String { get; init; }
        [VNotEmpty] public int[]? Array { get; init; }
        [VNotEmpty] public List<int>? List { get; init; }
        [VNotEmpty] public ArrayList? ArrayList { get; init; }
        [VNotEmpty] public IEnumerable<int>? Enumerable { get; init; }
    }

    [Validatable]
    partial struct LengthTest
    {
        [VLength(1, 10)] public string? String { get; init; }
        [VLength(1, 10)] public int[]? Array { get; init; }
        [VLength(1, 10)] public List<int>? List { get; init; }
        [VLength(1, 10)] public ArrayList? ArrayList { get; init; }
    }

    static void Main()
    {
        RunNotEmptyTest();

        Console.WriteLine("\n----------------------------------------\n");

        RunLengthTest();

        Console.WriteLine("\n----------------------------------------\n");

        RegexTest.Run();
    }

    static void RunNotEmptyTest()
    {
        NotEmptyTest instance = new()
        {
            String = "",
            Array = [1, 2, 3, 4, 5],
            List = [],
            ArrayList = [1, 'A', "Banana"],
            Enumerable = null // null은 스킵
        };

        var result = instance.Validate();

        Console.WriteLine($"===== NotEmpty Test =====");
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

    static void RunLengthTest()
    {
        LengthTest instance = new()
        {
            String = "01234567890",
            Array = [0],
            List = [],
            ArrayList = null // null은 스킵
        };

        var result = instance.Validate();

        Console.WriteLine($"===== Length Test =====");
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

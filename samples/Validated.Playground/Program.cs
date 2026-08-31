using Validated.Annotations;

namespace Validated.Playground;

internal partial class Program
{
    static void Main()
    {
        NotEmptyTest.Run();
        Console.WriteLine("\n----------------------------------------\n");
        LengthTest.Run();
        Console.WriteLine("\n----------------------------------------\n");
        RegexTest.Run();
        Console.WriteLine("\n----------------------------------------\n");
        ComparisonTest.Run();
        Console.WriteLine("\n----------------------------------------\n");
        CustomMessageTest.Run();
        Console.WriteLine("\n----------------------------------------\n");

        CollectionTest instance = new(
            List: [
                new ElementTest(null, -333),
                new ElementTest("Apple", 404),
                new ElementTest("Banana", 0),
                new ElementTest(null, 3)
            ],
            Array: [
                new ElementTest(null, -2),
            ]
        );

        Console.WriteLine(string.Join('\n', instance.Validate().Errors.Select(e => $"[{e.PropertyName}] {e.Message} ({e.RuleName})")));
    }

    [Validatable]
    partial record ElementTest(
        [property: VNotNull] string? String,

        [property: VRange(-10, 10)] int Int
    );

    [Validatable]
    partial record CollectionTest(
        [property: ValidateCollection] List<ElementTest> List,

        [property: ValidateCollection] ElementTest[] Array
    );
}

using Validated.Annotations;

namespace Validated.Playground;

public partial class ComparisonTest
{
    enum MyEnum
    {
        Zero = 0,
        One = 1,
        Two = 2
    }

    [Validatable]
    partial record ComparisonTestRecord(
        [property: VGreaterThan<int>(10)] int Int,

        [property: VGreaterThanProperty("Int")] int BigInt,

        [property: VGreaterThan<string>("Banana")] string String,

        [property: VGreaterThan<MyEnum>(MyEnum.One)] MyEnum MyEnum
    );

    public static void Run()
    {
        ComparisonTestRecord instanceValid = new(
            Int: 15,
            BigInt: 20,
            String: "Carrot",
            MyEnum: MyEnum.Two
        );

        var validResult = instanceValid.Validate();

        Console.WriteLine($"===== ComparisonTest =====");
        Console.WriteLine($"IsValid: {validResult.IsValid}");

        ComparisonTestRecord instanceInvalid = new(
            Int: 10,
            BigInt: 10,
            String: "Apple",
            MyEnum: MyEnum.Zero
        );

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

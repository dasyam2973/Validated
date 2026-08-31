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

        [property: VEqualProperty("Int")] int IntInt,

        [property: VNotEqualProperty("Int")] int NotInt,

        [property: VLessThanOrEqual<string>("Banana")] string String,

        [property: VLessThan<MyEnum>(MyEnum.One)] MyEnum MyEnum
    );

    public static void Run()
    {
        ComparisonTestRecord instanceValid = new(
            Int: 15,
            BigInt: 20,
            IntInt: 15,
            NotInt: 0,
            String: "Apple",
            MyEnum: MyEnum.Zero
        );

        var validResult = instanceValid.Validate();

        Console.WriteLine($"===== ComparisonTest =====");
        Console.WriteLine($"IsValid: {validResult.IsValid}");

        ComparisonTestRecord instanceInvalid = new(
            Int: 10,
            BigInt: 10,
            IntInt: 9,
            NotInt: 10,
            String: "Carrot",
            MyEnum: MyEnum.One
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

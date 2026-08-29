using Validated.Annotations;

namespace Validated.Sample;

internal class Program
{
    static void Main()
    {
        BasicExample.Run1();
        Console.WriteLine("\n----------------------------------------\n");
        BasicExample.Run2();
    }
}

public static partial class BasicExample
{
    public static void Run1()
    {
        var user = new UserRegistration("a", 15);
        var result = user.Validate();

        Console.WriteLine($"IsValid: {result.IsValid}"); // -> False

        foreach (var error in result.Errors)
        {
            Console.WriteLine($"- [{error.PropertyName}] {error.Message}");
        }
    }

    public static void Run2()
    {
        var user = new UserRegistration("john", 20);

        Console.WriteLine($"IsValid: {user.IsValid}"); // -> True
    }

    [Validatable]
    public partial record UserRegistration(
        [property: VLength(3, 20)] string Username,
        [property: VRange(18, 100)] int Age
    );
}
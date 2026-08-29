using Validated.Annotations;

namespace Validated.Sample;

partial class Program
{
    [Validatable]
    public partial record UserRegistrationRequest(
        [property: VLength(3, 20)]
        string Username,

        [property: VRange(18, 100)]
        int Age,

        string Password,

        [property: VEqual("Password")]
        string ConfirmPassword,

        DateTime StartDate,

        [property: VGreaterThanOrEqual("StartDate")]
        DateTime EndDate
    );

    static void Main()
    {
        Console.WriteLine("=== Source Generator Validation Sample ===\n");

        var validUser = new UserRegistrationRequest(
            Username: "john_doe",
            Age: 25,
            Password: "Password123!",
            ConfirmPassword: "Password123!",
            StartDate: DateTime.Now,
            EndDate: DateTime.Now.AddDays(7)
        );

        var validResult = validUser.Validate();
        Console.WriteLine($"Valid User Check: {validResult.IsValid}"); // -> True

        Console.WriteLine("\n----------------------------------------\n");

        var invalidUser = new UserRegistrationRequest(
            Username: "a",
            Age: 15,
            Password: "Password123!",
            ConfirmPassword: "MismatchPassword!",
            StartDate: DateTime.Now.AddDays(5),
            EndDate: DateTime.Now
        );

        var invalidResult = invalidUser.Validate();
        Console.WriteLine($"Invalid User Check: {invalidResult.IsValid}"); // -> False

        Console.WriteLine("\n[Validation Failures]");
        foreach (var error in invalidResult.Errors)
        {
            Console.WriteLine($"- [{error.PropertyName}] {error.Message}");
        }
    }
}
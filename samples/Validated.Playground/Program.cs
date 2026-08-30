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
    }
}

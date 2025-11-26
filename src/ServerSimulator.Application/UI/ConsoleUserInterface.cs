namespace ServerSimulator.Application.UI;

public class ConsoleUserInterface : IUserInterface
{
    public void ShowHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=========================================");
        Console.WriteLine("   🚀 SERVER SIMULATOR CLI v1.1   ");
        Console.WriteLine("=========================================");
        Console.ResetColor();
    }

    public void ShowInfo(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }

    public void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ {message}");
        Console.ResetColor();
    }

    public string PromptString(string message, string defaultValue)
    {
        Console.Write($"{message} [default: {defaultValue}]: ");
        var input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
    }

    public int PromptInt(string message, int defaultValue)
    {
        while (true)
        {
            Console.Write($"{message} [default: {defaultValue}]: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return defaultValue;

            if (int.TryParse(input, out int result) && result > 0)
                return result;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️ Invalid input. Please enter a positive number.");
            Console.ResetColor();
        }
    }

    public void WaitForKeyPress()
    {
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerSimulator.Library.Configurations;
using ServerSimulator.Library.Factories;
using ServerSimulator.Library.Logging;
using ServerSimulator.Library.Services;
using ServerSimulator.Library.Services.ServerWorkloadManager;
using ServerSimulator.Library.Services.SnapshotService;
using ServerSimulator.Library.Validators;

var services = new ServiceCollection();

services.AddLogging(configure => 
{
    configure.AddConsole();
    configure.SetMinimumLevel(LogLevel.Information);
});

services.AddTransient<IServerConfigurationProvider, ServerConfigurationProvider>();
services.AddTransient<IServerFactory, ServerFactory>();
services.AddTransient<IServerWorkloadManager, ServerWorkloadManager>();
services.AddTransient<ServerInitializer>();
services.AddTransient<ServerManager>();
services.AddTransient<IValidator<ServerConfiguration>, ServerConfigurationValidator>();

services.AddSingleton<SnapshotLogger>();
services.AddSingleton<ISnapshotService>(sp => 
    new SnapshotService(sp.GetRequiredService<SnapshotLogger>(), TimeSpan.FromSeconds(2)));

services.AddTransient<ServerSimulator.Application.ServerSimulator>();

var provider = services.BuildServiceProvider();

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("=========================================");
Console.WriteLine("   SERVER SIMULATOR CLI v1.0   ");
Console.WriteLine("=========================================");
Console.ResetColor();

try
{
    var app = provider.GetRequiredService<ServerSimulator.Application.ServerSimulator>();

    string configPath = PromptString("Enter configuration file path", "servers_config.json");
    
    if (!File.Exists(configPath))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Error: Configuration file '{configPath}' not found.");
        Console.ResetColor();
        return;
    }

    int playerCount = PromptInt("Enter number of players per server", 50);
    int duration = PromptInt("Enter max connection duration (ms)", 3000);

    Console.WriteLine($"\n[INFO] Initializing servers from: {configPath}...");
    app.Initialize(configPath);

    Console.WriteLine($"[INFO] Starting simulation for {playerCount} players...");
    await app.RunSimulation(playerCount, duration);
    
    Console.WriteLine("\n✅ Simulation finished.");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n🔥 CRITICAL ERROR: {ex.Message}");
    Console.ResetColor();
}
finally
{
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

static string PromptString(string message, string defaultValue)
{
    Console.Write($"{message} [default: {defaultValue}]: ");
    var input = Console.ReadLine();
    return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
}

static int PromptInt(string message, int defaultValue)
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
        Console.WriteLine("Invalid input. Please enter a positive number.");
        Console.ResetColor();
    }
}
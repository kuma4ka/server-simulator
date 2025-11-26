using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServerSimulator.Application.UI;
using ServerSimulator.Library.Configurations;
using ServerSimulator.Library.Factories;
using ServerSimulator.Library.Logging;
using ServerSimulator.Library.Services;
using ServerSimulator.Library.Services.ServerWorkloadManager;
using ServerSimulator.Library.Services.SnapshotService;
using ServerSimulator.Library.Validators;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

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
services.AddTransient<IUserInterface, ConsoleUserInterface>();
services.AddTransient<ServerSimulator.Application.ServerSimulator>();

services.AddSingleton<SnapshotLogger>();
services.AddSingleton<ISnapshotService>(sp => 
    new SnapshotService(sp.GetRequiredService<SnapshotLogger>(), TimeSpan.FromSeconds(2)));


var provider = services.BuildServiceProvider();

var ui = provider.GetRequiredService<IUserInterface>();
var app = provider.GetRequiredService<ServerSimulator.Application.ServerSimulator>();

ui.ShowHeader();

try
{
    string configPath = ui.PromptString("Enter configuration file path", "servers_config.json");
    
    if (!File.Exists(configPath))
    {
        ui.ShowError($"Configuration file '{configPath}' not found.");
        return;
    }

    int playerCount = ui.PromptInt("Enter number of players per server", 50);
    int duration = ui.PromptInt("Enter max connection duration (ms)", 3000);

    ui.ShowInfo($"Initializing servers from: {configPath}...");
    app.Initialize(configPath);

    ui.ShowInfo($"Starting simulation for {playerCount} players...");
    await app.RunSimulation(playerCount, duration);
    
    ui.ShowInfo("Simulation finished successfully.");
}
catch (Exception ex)
{
    ui.ShowError($"CRITICAL ERROR: {ex.Message}");
}
finally
{
    ui.WaitForKeyPress();
}
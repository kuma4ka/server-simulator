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

services.AddLogging(c => c.AddConsole());
services.AddTransient<IServerConfigurationProvider, ServerConfigurationProvider>();
services.AddTransient<IServerFactory, ServerFactory>();
services.AddTransient<IServerWorkloadManager, ServerWorkloadManager>();
services.AddTransient<ServerInitializer>();
services.AddTransient<ServerManager>();
services.AddTransient<IValidator<ServerConfiguration>, ServerConfigurationValidator>();
services.AddSingleton<SnapshotLogger>();
services.AddSingleton<ISnapshotService>(sp => 
    new SnapshotService(sp.GetRequiredService<SnapshotLogger>(), TimeSpan.FromSeconds(3)));
services.AddTransient<ServerSimulator.Application.ServerSimulator>();

var provider = services.BuildServiceProvider();
var app = provider.GetRequiredService<ServerSimulator.Application.ServerSimulator>();

Console.WriteLine("Starting simulation...");

string configPath = "servers_config.json"; 
if(File.Exists(configPath))
{
    app.Initialize(configPath);
    await app.RunSimulation(50, 3000);
}
else
{
    Console.WriteLine("Config file not found. Please create servers_config.json");
}
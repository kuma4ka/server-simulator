using Microsoft.Extensions.Logging;
using ServerSimulator.Library.Entities;
using ServerSimulator.Library.Services;
using ServerSimulator.Library.Interfaces;

namespace ServerSimulator.Application;

public class ServerSimulator(
    ILogger<Server> serverLogger,
    ServerInitializer initializer,
    ISnapshotService snapshotService,
    ServerManager serverManager)
{
    private List<Server> _servers = [];

    public void Initialize(string path)
    {
        _servers = initializer.InitializeServers(path, serverLogger);
        snapshotService.Start(_servers);
    }

    public async Task RunSimulation(int players, int duration)
    {
        if (_servers.Count == 0) return;
        await serverManager.SimulateConnectionsAsync(_servers, players, duration);
        await Task.Delay(2000); // Wait for logs
    }
}
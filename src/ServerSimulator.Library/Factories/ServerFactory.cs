using Microsoft.Extensions.Logging;
using ServerSimulator.Library.Configurations;
using ServerSimulator.Library.Entities;
using ServerSimulator.Library.Services.ServerWorkloadManager;

namespace ServerSimulator.Library.Factories;

public interface IServerFactory
{
    Server CreateServer(ServerConfiguration configuration, ILogger<Server> logger);
}

public class ServerFactory(IServerWorkloadManager workloadManager) : IServerFactory
{
    public Server CreateServer(ServerConfiguration configuration, ILogger<Server> logger)
    {
        return new Server(configuration, logger, workloadManager);
    }
}
using Microsoft.Extensions.Logging;
using ServerSimulator.Library.Configurations;
using ServerSimulator.Library.Entities;

namespace ServerSimulator.Library.Interfaces;

public interface IServerFactory
{
    Server CreateServer(ServerConfiguration configuration, ILogger<Server> logger);
}
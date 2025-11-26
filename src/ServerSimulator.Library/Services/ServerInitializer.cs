using FluentValidation;
using Microsoft.Extensions.Logging;
using ServerSimulator.Library.Configurations;
using ServerSimulator.Library.Entities;
using ServerSimulator.Library.Interfaces;

namespace ServerSimulator.Library.Services;

public class ServerInitializer(
    IServerConfigurationProvider configProvider,
    IServerFactory factory,
    IValidator<ServerConfiguration> validator)
{
    public List<Server> InitializeServers(string path, ILogger<Server> logger)
    {
        var configs = configProvider.GetConfigurations(path);
        var servers = new List<Server>();
        foreach (var cfg in configs)
        {
            if (validator.Validate(cfg).IsValid) servers.Add(factory.CreateServer(cfg, logger));
        }
        return servers;
    }
}
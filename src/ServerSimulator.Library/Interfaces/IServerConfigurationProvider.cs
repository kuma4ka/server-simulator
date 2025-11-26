using ServerSimulator.Library.Configurations;

namespace ServerSimulator.Library.Interfaces;

public interface IServerConfigurationProvider
{
    List<ServerConfiguration> GetConfigurations(string path);
}
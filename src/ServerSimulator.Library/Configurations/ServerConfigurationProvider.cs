using Newtonsoft.Json;
using ServerSimulator.Library.Interfaces;

namespace ServerSimulator.Library.Configurations;

public class ServerConfigurationProvider : IServerConfigurationProvider
{
    public List<ServerConfiguration> GetConfigurations(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"Config not found: {path}");
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<ServerConfiguration>>(json) ?? [];
    }
}
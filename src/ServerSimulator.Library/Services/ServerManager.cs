using ServerSimulator.Library.Entities;

namespace ServerSimulator.Library.Services;

public class ServerManager
{
    public async Task SimulateConnectionsAsync(List<Server> servers, int players, int duration)
    {
        var tasks = new List<Task>();
        var random = new Random();
        foreach (var server in servers)
        {
            for (int i = 0; i < players; i++)
            {
                tasks.Add(Task.Run(async () => {
                    if (await server.ConnectPlayerAsync())
                    {
                        await Task.Delay(random.Next(duration));
                        server.DisconnectPlayer();
                    }
                }));
            }
        }
        await Task.WhenAll(tasks);
    }
}
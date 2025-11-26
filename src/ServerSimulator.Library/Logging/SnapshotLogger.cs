using Microsoft.Extensions.Logging;
using ServerSimulator.Library.Entities;
using System.Text;

namespace ServerSimulator.Library.Logging;

public class SnapshotLogger(ILogger<SnapshotLogger> logger)
{
    public void LogSnapshots(List<Server> servers)
    {
        var sb = new StringBuilder();
        sb.AppendLine("--- SNAPSHOT ---");
        foreach (var s in servers)
        {
            var snap = s.GetSnapshot();
            sb.AppendLine($"[{DateTime.Now:T}] {snap.Name}: {snap.CurrentOccupancy}/{snap.MaxCapacity}");
        }
        logger.LogInformation("{Snapshot}", sb.ToString());
    }
}
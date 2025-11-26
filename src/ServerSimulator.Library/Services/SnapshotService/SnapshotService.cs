using ServerSimulator.Library.Entities;
using ServerSimulator.Library.Logging;

namespace ServerSimulator.Library.Services.SnapshotService;

public interface ISnapshotService { void Start(List<Server> servers); }

public class SnapshotService(SnapshotLogger snapshotLogger, TimeSpan period) : ISnapshotService, IDisposable
{
    private readonly PeriodicTimer _timer = new(period);
    private CancellationTokenSource? _cts;

    public void Start(List<Server> servers)
    {
        _cts = new CancellationTokenSource();
        Task.Run(async () => {
            while (await _timer.WaitForNextTickAsync(_cts.Token)) snapshotLogger.LogSnapshots(servers);
        });
    }

    public void Dispose() { _cts?.Cancel(); _timer.Dispose(); }
}
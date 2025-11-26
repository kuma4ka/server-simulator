using Microsoft.Extensions.Logging;
using ServerSimulator.Library.Configurations;
using ServerSimulator.Library.Interfaces;

namespace ServerSimulator.Library.Entities;

public class Server
{
    private readonly ServerConfiguration _configuration;
    private readonly ILogger<Server> _logger;
    private readonly IServerWorkloadManager _workloadManager;
    private readonly SemaphoreSlim _semaphore;

    private int _currentOccupancy;
    private bool _isHighLoad;

    public Server(
        ServerConfiguration configuration,
        ILogger<Server> logger,
        IServerWorkloadManager workloadManager)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _workloadManager = workloadManager ?? throw new ArgumentNullException(nameof(workloadManager));

        _currentOccupancy = configuration.CurrentOccupancy;
        _semaphore = new SemaphoreSlim(configuration.MaxCapacity, configuration.MaxCapacity);
        
        for (int i = 0; i < _currentOccupancy; i++)
        {
            _semaphore.WaitAsync().GetAwaiter().GetResult();
        }
    }

    public async Task<bool> ConnectPlayerAsync()
    {
        if (_currentOccupancy >= _configuration.MaxCapacity)
        {
            _logger.LogWarning("Connection refused: Server {Name} is full.", _configuration.Name);
            return false;
        }

        await _semaphore.WaitAsync();
        Interlocked.Increment(ref _currentOccupancy);
        _logger.LogInformation("Player connected to {Name}. Occupancy: {Occupancy}/{Max}", _configuration.Name, _currentOccupancy, _configuration.MaxCapacity);
        UpdateLoadState();
        return true;
    }

    public void DisconnectPlayer()
    {
        if (_currentOccupancy <= 0) return;
        Interlocked.Decrement(ref _currentOccupancy);
        _semaphore.Release();
        _logger.LogInformation("Player disconnected from {Name}. Occupancy: {Occupancy}/{Max}", _configuration.Name, _currentOccupancy, _configuration.MaxCapacity);
        UpdateLoadState();
    }

    public ServerConfiguration GetSnapshot() => _configuration with { CurrentOccupancy = _currentOccupancy };

    private void UpdateLoadState()
    {
        _isHighLoad = _workloadManager.CheckAndLogLoadState(_isHighLoad, _currentOccupancy, _configuration.MaxCapacity, _configuration.LoadThreshold);
    }
}
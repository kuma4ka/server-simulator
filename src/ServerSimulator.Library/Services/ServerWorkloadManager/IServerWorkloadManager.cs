namespace ServerSimulator.Library.Services.ServerWorkloadManager;

public interface IServerWorkloadManager
{
    bool CheckAndLogLoadState(bool wasHighLoad, int currentOccupancy, int maxCapacity, float threshold);
}
namespace ServerSimulator.Library.Interfaces;

public interface IServerWorkloadManager
{
    bool CheckAndLogLoadState(bool wasHighLoad, int currentOccupancy, int maxCapacity, float threshold);
}
using Microsoft.Extensions.Logging;

namespace ServerSimulator.Library.Services.ServerWorkloadManager;

public class ServerWorkloadManager(ILogger<ServerWorkloadManager> logger) : IServerWorkloadManager
{
    public bool CheckAndLogLoadState(bool wasHighLoad, int currentOccupancy, int maxCapacity, float threshold)
    {
        bool isNowHighLoad = currentOccupancy >= (threshold * maxCapacity);

        if (isNowHighLoad && !wasHighLoad)
            logger.LogWarning("⚠️ HIGH LOAD ALERT: Occupancy {Occupancy}/{Max}", currentOccupancy, maxCapacity);
        else if (!isNowHighLoad && wasHighLoad)
            logger.LogInformation("✅ LOAD NORMALIZED: Occupancy {Occupancy}/{Max}", currentOccupancy, maxCapacity);

        return isNowHighLoad;
    }
}
using ServerSimulator.Library.Entities;

namespace ServerSimulator.Library.Interfaces;

public interface ISnapshotService
{
    void Start(List<Server> servers);
}
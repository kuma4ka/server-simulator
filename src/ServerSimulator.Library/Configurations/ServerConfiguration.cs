namespace ServerSimulator.Library.Configurations;

public record ServerConfiguration(
    int MaxCapacity,
    int CurrentOccupancy,
    string ServerId,
    string Name,
    string Ip
);
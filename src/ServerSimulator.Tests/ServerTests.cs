using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using ServerSimulator.Library.Configurations;
using ServerSimulator.Library.Entities;
using ServerSimulator.Library.Interfaces;

namespace ServerSimulator.Tests;

public class ServerTests
{
    [Fact]
    public async Task ConnectPlayerAsync_ShouldReturnFalse_WhenServerIsFull()
    {
        // Arrange
        var config = new ServerConfiguration(
            MaxCapacity: 10,
            CurrentOccupancy: 10, 
            ServerId: "TEST-FULL",
            Name: "Full Server",
            Ip: "127.0.0.1"
        );

        var logger = NullLogger<Server>.Instance; 
        var workloadManager = Substitute.For<IServerWorkloadManager>();

        var server = new Server(config, logger, workloadManager);

        // Act
        var result = await server.ConnectPlayerAsync();

        // Assert
        result.Should().BeFalse("because the server is completely full");
    }

    [Fact]
    public async Task ConnectPlayerAsync_ShouldReturnTrue_WhenServerHasSpace()
    {
        // Arrange
        var config = new ServerConfiguration(10, 5, "TEST-OPEN", "Open Server", "127.0.0.1");
        var server = new Server(config, NullLogger<Server>.Instance, Substitute.For<IServerWorkloadManager>());

        // Act
        var result = await server.ConnectPlayerAsync();

        // Assert
        result.Should().BeTrue("because there are free slots on the server");
    }
    
    [Fact]
    public async Task DisconnectPlayer_ShouldDecreaseOccupancy_AndReleaseSlot()
    {
        // Arrange
        var config = new ServerConfiguration(10, 1, "TEST-3", "Test Server", "127.0.0.1");
        var server = new Server(config, NullLogger<Server>.Instance, Substitute.For<IServerWorkloadManager>());

        server.GetSnapshot().CurrentOccupancy.Should().Be(1);

        // Act
        server.DisconnectPlayer();

        // Assert
        server.GetSnapshot().CurrentOccupancy.Should().Be(0, "the player left");
        
        int connectedCount = 0;
        for (int i = 0; i < 10; i++)
        {
            if (await server.ConnectPlayerAsync()) connectedCount++;
        }
        connectedCount.Should().Be(10, "we have freed up space, so the server should accept the full number of new players.");
    }
}
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using ServerSimulator.Library.Services.ServerWorkloadManager;

namespace ServerSimulator.Tests;

public class ServerWorkloadManagerTests
{
    private readonly ServerWorkloadManager _manager = new(NullLogger<ServerWorkloadManager>.Instance);

    [Theory]
    [InlineData(80, 100, 0.8, true)]
    [InlineData(81, 100, 0.8, true)]
    [InlineData(79, 100, 0.8, false)]
    public void CheckAndLogLoadState_ShouldDetectHighLoad_Correctly(
        int occupancy, int capacity, float threshold, bool expectedResult)
    {
        // Act
        var result = _manager.CheckAndLogLoadState(false, occupancy, capacity, threshold);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void CheckAndLogLoadState_ShouldReturnFalse_WhenLoadDrops()
    {
        // Arrange
        bool wasHighLoad = true;
        int occupancy = 50;
        int capacity = 100;

        // Act
        var result = _manager.CheckAndLogLoadState(wasHighLoad, occupancy, capacity, 0.8f);

        // Assert
        result.Should().BeFalse("the load has dropped, the status should change to false");
    }
}
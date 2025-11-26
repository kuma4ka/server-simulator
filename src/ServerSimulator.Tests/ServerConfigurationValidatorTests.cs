using FluentAssertions;
using ServerSimulator.Library.Configurations;
using ServerSimulator.Library.Validators;

namespace ServerSimulator.Tests;

public class ServerConfigurationValidatorTests
{
    private readonly ServerConfigurationValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_ForValidConfiguration()
    {
        var config = new ServerConfiguration(100, 0, "S-1", "Valid Server", "192.168.1.1", 0.8f);
        var result = _validator.Validate(config);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Validate_ShouldFail_WhenMaxCapacityIsInvalid(int capacity)
    {
        var config = new ServerConfiguration(capacity, 0, "S-1", "Name", "127.0.0.1", 0.8f);
        var result = _validator.Validate(config);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ServerConfiguration.MaxCapacity));
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-ip")]
    public void Validate_ShouldFail_WhenIpIsInvalid(string invalidIp)
    {
        var config = new ServerConfiguration(100, 0, "S-1", "Name", invalidIp, 0.8f);
        var result = _validator.Validate(config);
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ServerConfiguration.Ip));
    }

    [Theory]
    [InlineData(0.05f)]
    [InlineData(1.1f)]
    public void Validate_ShouldFail_WhenLoadThresholdIsInvalid(float threshold)
    {
        var config = new ServerConfiguration(100, 0, "S-1", "Name", "127.0.0.1", threshold);
        var result = _validator.Validate(config);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ServerConfiguration.LoadThreshold));
    }
}
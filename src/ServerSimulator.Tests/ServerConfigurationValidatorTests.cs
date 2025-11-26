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
        // Arrange
        var config = new ServerConfiguration(100, 0, "S-1", "Valid Server", "192.168.1.1");

        // Act
        var result = _validator.Validate(config);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Validate_ShouldFail_WhenMaxCapacityIsInvalid(int capacity)
    {
        var config = new ServerConfiguration(capacity, 0, "S-1", "Name", "127.0.0.1");
        var result = _validator.Validate(config);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ServerConfiguration.MaxCapacity));
    }

    [Theory]
    [InlineData("")]
    [InlineData("999.999.999")]
    [InlineData("not-an-ip")]
    public void Validate_ShouldFail_WhenIpIsInvalid(string invalidIp)
    {
        var config = new ServerConfiguration(100, 0, "S-1", "Name", invalidIp);
        var result = _validator.Validate(config);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ServerConfiguration.Ip));
    }
}
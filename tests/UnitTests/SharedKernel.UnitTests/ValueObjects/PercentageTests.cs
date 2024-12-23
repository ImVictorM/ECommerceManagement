using FluentAssertions;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace SharedKernel.UnitTests.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="Percentage"/> value object.
/// </summary>
public class PercentageTests
{
    /// <summary>
    /// Verifies that a <see cref="Percentage"/> is created successfully with a valid value.
    /// </summary>
    [Fact]
    public void Percentage_WhenCreatedWithValidValue_CreatesInstanceCorrectly()
    {
        var validValue = 50;

        var percentage = PercentageUtils.Create(validValue);

        percentage.Should().NotBeNull();
        percentage.Value.Should().Be(validValue);
    }

    /// <summary>
    /// Verifies that the <see cref="Percentage.IsWithinRange(int, int)"/> method returns true when the value is within the specified range.
    /// </summary>
    [Fact]
    public void Percentage_WhenValueIsWithinRange_ReturnsTrue()
    {
        var value = 50;
        var percentage = Percentage.Create(value);

        var result = percentage.IsWithinRange(0, 100);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the <see cref="Percentage.IsWithinRange(int, int)"/> method returns false when the value is outside the specified range.
    /// </summary>
    [Fact]
    public void Percentage_WhenValueIsOutsideRange_ReturnsFalse()
    {
        var value = 150;
        var percentage = Percentage.Create(value);

        var result = percentage.IsWithinRange(0, 100);

        result.Should().BeFalse();
    }

    /// <summary>
    /// Verifies that the <see cref="Percentage.IsWithinRange(int)"/> method returns true when the value is within the default range (0 to end).
    /// </summary>
    [Fact]
    public void Percentage_WhenValueIsWithinDefaultRange_ReturnsTrue()
    {
        var value = 25;
        var percentage = Percentage.Create(value);

        var result = percentage.IsWithinRange(50);

        result.Should().BeTrue();
    }

    /// <summary>
    /// Verifies that the <see cref="Percentage.IsWithinRange(int)"/> method returns false when the value is outside the default range (0 to end).
    /// </summary>
    [Fact]
    public void Percentage_WhenValueIsOutsideDefaultRange_ReturnsFalse()
    {
        var value = 75;
        var percentage = Percentage.Create(value);

        var result = percentage.IsWithinRange(50);

        result.Should().BeFalse();
    }
}

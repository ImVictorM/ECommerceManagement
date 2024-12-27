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
    public void CreatePercentage_WithValidValue_CreatesInstanceCorrectly()
    {
        var validValue = 50;

        var percentage = PercentageUtils.Create(validValue);

        percentage.Should().NotBeNull();
        percentage.Value.Should().Be(validValue);
    }

    /// <summary>
    /// Verifies that the <see cref="Percentage.IsWithinRange(int, int)"/> method the expected result.
    /// </summary>
    [Theory]
    [InlineData(50, true)]
    [InlineData(101, false)]
    [InlineData(100, true)]
    [InlineData(0, true)]
    [InlineData(-1, false)]
    public void IsWithinRange_WhenCheckingWithValidValue_ReturnsExpected(int value, bool expected)
    {
        var percentage = Percentage.Create(value);

        var result = percentage.IsWithinRange(0, 100);

        result.Should().Be(expected);
    }

    /// <summary>
    /// Verifies that the percentage comparison is correct when sorting by descending percentages.
    /// </summary>
    [Fact]
    public void PercentageComparison_WhenSortingDescending_ReturnsExpected()
    {
        var percentages = new[]
        {
            Percentage.Create(50),
            Percentage.Create(10),
            Percentage.Create(90),
            Percentage.Create(30)
        };

        var expectedAfterSort = new[]
        {
            Percentage.Create(90),
            Percentage.Create(50),
            Percentage.Create(30),
            Percentage.Create(10),
        };

        var sorted = percentages.OrderByDescending(p => p).ToArray();

        sorted.Should().Equal(expectedAfterSort);
    }

    /// <summary>
    /// Verifies that the percentage comparison is correct when sorting by ascending percentages.
    /// </summary>
    [Fact]
    public void PercentageComparison_WhenSortingAscending_ReturnsExpected()
    {
        var percentages = new[]
        {
            Percentage.Create(50),
            Percentage.Create(10),
            Percentage.Create(90),
            Percentage.Create(30)
        };

        var expectedAfterSort = new[]
        {
            Percentage.Create(10),
            Percentage.Create(30),
            Percentage.Create(50),
            Percentage.Create(90),
        };

        var sorted = percentages.OrderBy(p => p).ToArray();

        sorted.Should().Equal(expectedAfterSort);
    }
}

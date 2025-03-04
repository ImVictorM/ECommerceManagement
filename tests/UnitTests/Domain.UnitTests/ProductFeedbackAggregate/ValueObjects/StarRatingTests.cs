using Domain.ProductFeedbackAggregate.Errors;
using Domain.ProductFeedbackAggregate.ValueObjects;

using FluentAssertions;

namespace Domain.UnitTests.ProductFeedbackAggregate.ValueObjects;

/// <summary>
/// Unit tests for the <see cref="StarRating"/> value object.
/// </summary>
public class StarRatingTests
{
    /// <summary>
    /// Verifies creating a star rating with a value between 0 and 5
    /// creates the instance correctly.
    /// </summary>
    /// <param name="value">The value between 0 and 5.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void CreateStarRating_WithValidValue_ReturnsInstance(int value)
    {
        var action = FluentActions
            .Invoking(() => StarRating.Create(value))
            .Should()
            .NotThrow();

        var starRating = action.Subject;

        starRating.Value.Should().Be(value);
    }

    /// <summary>
    /// Verifies an exception is thrown when the star rating value is not between
    /// 0 and 5.
    /// </summary>
    /// <param name="value">The invalid rating value.</param>
    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    [InlineData(10)]
    public void CreateStarRating_WithValueOutOfValidRange_ThrowsError(int value)
    {
        FluentActions
            .Invoking(() => StarRating.Create(value))
            .Should()
            .Throw<InvalidStarRatingRangeException>();
    }
}

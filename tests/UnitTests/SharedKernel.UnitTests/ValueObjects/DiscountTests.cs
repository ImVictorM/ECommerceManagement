using SharedKernel.Errors;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;

namespace SharedKernel.UnitTests.ValueObjects;

/// <summary>
/// Tests for the <see cref="Discount"/> value object.
/// </summary>
public class DiscountTests
{
    /// <summary>
    /// List of valid starting and ending dates.
    /// </summary>
    /// <returns>A list of valid starting and ending dates.</returns>
    public static IEnumerable<object[]> ValidDiscountDates()
    {
        yield return new object[] {
             DateTimeOffset.UtcNow.AddHours(10),
             DateTimeOffset.UtcNow.AddDays(1).AddHours(20)
        };
        yield return new object[] {

            DateTimeOffset.UtcNow.AddDays(2),
            DateTimeOffset.UtcNow.AddDays(2).AddHours(10)
        };
    }

    /// <summary>
    /// List of invalid starting and ending dates.
    /// </summary>
    /// <returns>A list of invalid starting and ending dates.</returns>
    public static IEnumerable<object[]> InvalidDiscountDates()
    {
        yield return new object[] {
            DateTimeOffset.UtcNow.AddDays(-1).AddHours(-23),
            DateTimeOffset.UtcNow.AddDays(2),
        };
        yield return new object[] {
            DateTimeOffset.UtcNow.AddDays(2),
            DateTimeOffset.UtcNow.AddDays(2).AddMinutes(59),
        };
    }

    /// <summary>
    /// Defines pairs of discount and the expected return value when validating if the discount is valid to date.
    /// </summary>
    public static IEnumerable<object[]> DiscountAndExpectedValidToDatePairs()
    {
        yield return new object[]
        {
            DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddHours(-2),
                endingDate: DateTimeOffset.UtcNow.AddDays(1)
            ),
            true
        };

        yield return new object[]
        {
            DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddHours(-20),
                endingDate: DateTimeOffset.UtcNow.AddHours(-5)
            ),
            false
        };

        yield return new object[]
        {
            DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddDays(4),
                endingDate: DateTimeOffset.UtcNow.AddDays(10)
            ),
            false
        };
    }

    /// <summary>
    /// Tests if it is possible to create a new instance of discount with valid parameters.
    /// </summary>
    /// <param name="startingDate">The starting date of the discount.</param>
    /// <param name="endingDate">The ending date of the discount.</param>
    [Theory]
    [MemberData(nameof(ValidDiscountDates))]
    public void CreateDiscount_WithValidStartingAndEndingDate_ReturnsNewInstance(DateTimeOffset startingDate, DateTimeOffset endingDate)
    {
        Action act = () => DiscountUtils.CreateDiscount(startingDate: startingDate, endingDate: endingDate);

        act.Should().NotThrow();
    }

    /// <summary>
    /// Tests if it throws an error when the dates are invalid.
    /// </summary>
    /// <param name="startingDate">The starting date of the discount.</param>
    /// <param name="endingDate">The ending date of the discount.</param>
    [Theory]
    [MemberData(nameof(InvalidDiscountDates))]
    public void CreateDiscount_WithInvalidStartingAndEndingDate_ThrowsError(
        DateTimeOffset startingDate,
        DateTimeOffset endingDate
    )
    {
        Action act = () => DiscountUtils.CreateDiscount(startingDate: startingDate, endingDate: endingDate);

        act.Should().Throw<DomainValidationException>().WithMessage("The date range between the starting and ending date is invalid");
    }

    /// <summary>
    /// Tests if it throws an error when the percentage of the discount is not between 1 and 100.
    /// </summary>
    /// <param name="percentage">The discount percentage.</param>
    [Theory]
    [InlineData(-5)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(101)]
    [InlineData(200)]
    public void CreateDiscount_WithInvalidPercentage_ThrowsError(int percentage)
    {
        FluentActions
            .Invoking(() => DiscountUtils.CreateDiscount(percentage: PercentageUtils.Create(percentage)))
            .Should()
            .Throw<DomainValidationException>()
            .WithMessage("Discount percentage must be between 1 and 100");
    }

    /// <summary>
    /// Tests if the discount is valid to date.
    /// </summary>
    /// <param name="discount">The discount.</param>
    /// <param name="expectedValidToDate">The expected value indicating if the discount should be applied.</param>
    [Theory]
    [MemberData(nameof(DiscountAndExpectedValidToDatePairs))]
    public void IsValidToDate_WhenChecking_ReturnsExpected(Discount discount, bool expectedValidToDate)
    {
        discount.IsValidToDate.Should().Be(expectedValidToDate);
    }
}

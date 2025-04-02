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
    /// Provides a list of valid starting and ending dates.
    /// </summary>
    public static readonly IEnumerable<object[]> ValidDiscountDates =
    [
        [
            DateTimeOffset.UtcNow.AddHours(10),
             DateTimeOffset.UtcNow.AddDays(1).AddHours(20)
        ],
        [
            DateTimeOffset.UtcNow.AddDays(2),
            DateTimeOffset.UtcNow.AddDays(2).AddHours(10)
        ],
    ];

    /// <summary>
    /// Provides a list of invalid starting and ending dates.
    /// </summary>
    public static readonly IEnumerable<object[]> InvalidDiscountDates =
    [
        [
            DateTimeOffset.UtcNow.AddDays(-1).AddHours(-23),
            DateTimeOffset.UtcNow.AddDays(2),
        ],
        [
            DateTimeOffset.UtcNow.AddDays(2),
            DateTimeOffset.UtcNow.AddDays(2).AddMinutes(30),
        ],
    ];

    /// <summary>
    /// Provides pairs of discount and the expected return value when validating
    /// if the discount is valid to date.
    /// </summary>
    public static readonly IEnumerable<object[]> DiscountAndExpectedValidToDatePairs =
    [
        [
            DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddHours(-2),
                endingDate: DateTimeOffset.UtcNow.AddDays(1)
            ),
            true
        ],
        [
            DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddHours(-10),
                endingDate: DateTimeOffset.UtcNow.AddHours(-5)
            ),
            false
        ],
        [
             DiscountUtils.CreateDiscount(
                startingDate: DateTimeOffset.UtcNow.AddDays(4),
                endingDate: DateTimeOffset.UtcNow.AddDays(10)
            ),
            false
        ],
    ];

    /// <summary>
    /// Verifies it is possible to create a new discount instance with valid
    /// parameters.
    /// </summary>
    /// <param name="startingDate">The starting date of the discount.</param>
    /// <param name="endingDate">The ending date of the discount.</param>
    [Theory]
    [MemberData(nameof(ValidDiscountDates))]
    public void CreateDiscount_WithValidStartingAndEndingDate_CreatesWithoutThrowing(
        DateTimeOffset startingDate,
        DateTimeOffset endingDate
    )
    {
        var actionResult = FluentActions
            .Invoking(() => DiscountUtils.CreateDiscount(
                startingDate: startingDate,
                endingDate: endingDate
            ))
            .Should()
            .NotThrow();

        actionResult.Subject.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies an exception is thrown when the dates are invalid.
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
        FluentActions
            .Invoking(() => DiscountUtils.CreateDiscount(
                startingDate: startingDate,
                endingDate: endingDate
            ))
            .Should()
            .Throw<OutOfRangeException>()
            .WithMessage(
                "The date range between the starting and ending date is invalid"
            );
    }

    /// <summary>
    /// Verifies an exception is thrown when the percentage of the discount is
    /// not between 1 and 100.
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
            .Invoking(() => DiscountUtils.CreateDiscount(
                percentage: PercentageUtils.Create(percentage)
            ))
            .Should()
            .Throw<OutOfRangeException>()
            .WithMessage("Discount percentage must be between 1 and 100");
    }

    /// <summary>
    /// Verifies the <see cref="Discount.IsValidToDate"/> property returns the
    /// correct boolean value for different discounts.
    /// </summary>
    /// <param name="discount">The discount.</param>
    /// <param name="expectedValidToDate">
    /// The expected value indicating if the discount should be considered
    /// valid to date.
    /// </param>
    [Theory]
    [MemberData(nameof(DiscountAndExpectedValidToDatePairs))]
    public void IsValidToDate_WithDifferentDiscounts_ReturnsExpectedBoolean(
        Discount discount,
        bool expectedValidToDate
    )
    {
        discount.IsValidToDate.Should().Be(expectedValidToDate);
    }
}

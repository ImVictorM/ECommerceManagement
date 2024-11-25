using FluentAssertions;
using SharedKernel.Errors;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

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
        var now = DateTimeOffset.UtcNow;

        yield return new object[] {
             now.AddHours(10),
             now.AddDays(1).AddHours(20)
        };
        yield return new object[] {

            now.AddDays(2),
            now.AddDays(2).AddHours(10)
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
            "The starting date for the discount cannot be in the past"
        };
        yield return new object[] {
            DateTimeOffset.UtcNow.AddDays(2),
            DateTimeOffset.UtcNow.AddDays(2).AddMinutes(59),
            "The ending date and time must be at least one hour after the starting date"
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
    public void Discount_WhenCreatingNewDiscountWithValidParameters_ReturnsNewInstance(DateTimeOffset startingDate, DateTimeOffset endingDate)
    {
        Action act = () => DiscountUtils.CreateDiscount(startingDate: startingDate, endingDate: endingDate);

        act.Should().NotThrow();
    }

    /// <summary>
    /// Tests if it throws an error when the dates are invalid.
    /// </summary>
    /// <param name="startingDate">The starting date of the discount.</param>
    /// <param name="endingDate">The ending date of the discount.</param>
    /// <param name="expectedErrorMessage">The expected error message of the exception.</param>
    [Theory]
    [MemberData(nameof(InvalidDiscountDates))]
    public void Discount_WhenCreatingNewDiscountWithInvalidStartingAndEndDate_ThrowsAnError(
        DateTimeOffset startingDate,
        DateTimeOffset endingDate,
        string expectedErrorMessage
    )
    {
        Action act = () => DiscountUtils.CreateDiscount(startingDate: startingDate, endingDate: endingDate);

        act.Should().Throw<DomainValidationException>().WithMessage(expectedErrorMessage);
    }

    /// <summary>
    /// Tests if it throws an error when the percentage of the discount is not betweeen 1 and 100.
    /// </summary>
    /// <param name="percentage">The discount percentage.</param>
    [Theory]
    [InlineData(-5)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(101)]
    [InlineData(200)]
    public void Discount_WhenCreatingDiscountWithInvalidPercentage_ThrowsAnError(int percentage)
    {
        FluentActions
            .Invoking(() => DiscountUtils.CreateDiscount(percentage: percentage))
            .Should()
            .Throw<DomainValidationException>()
            .WithMessage("Discount percentage must be between 1 and 100");
    }

    /// <summary>
    /// Tests the method to validate if the current discount should be applied.
    /// </summary>
    /// <param name="discount">The discount.</param>
    /// <param name="expectedValidToDate">The expected value indicating if the discount should be applied.</param>
    [Theory]
    [MemberData(nameof(DiscountAndExpectedValidToDatePairs))]
    public void Discount_WhenCheckingIfDiscountIsValidToDate_ReturnsCorrectBooleanValue(Discount discount, bool expectedValidToDate)
    {
        var result = Discount.IsDiscountValidToDate(discount);

        result.Should().Be(expectedValidToDate);
    }
}

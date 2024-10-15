using Domain.Common.Errors;
using Domain.Common.ValueObjects;
using Domain.UnitTests.TestUtils;
using FluentAssertions;

namespace Domain.UnitTests.Common.ValueObjects;

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
             now.AddDays(1).AddHours(1),
             now.AddDays(1).AddHours(2)
        };
        yield return new object[] {

            now.AddDays(2),
            now.AddDays(2).AddHours(2)
        };
    }

    /// <summary>
    /// List of invalid starting and ending dates.
    /// </summary>
    /// <returns>A list of invalid starting and ending dates.</returns>
    public static IEnumerable<object[]> InvalidDiscountDates()
    {
        var now = DateTimeOffset.UtcNow;

        yield return new object[] {
            now.AddHours(23),
            now.AddDays(2),
            "The starting date for the discount must be at least one day in the future"
        };
        yield return new object[] {
            now.AddDays(2),
            now.AddDays(2).AddMinutes(59),
            "The ending date and time must be at least one hour after the starting date"
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
}

using Domain.Common.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Discount utilities.
/// </summary>
public static class DiscountUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Discount"/> class.
    /// </summary>
    /// <param name="percentage">The discount percentage.</param>
    /// <param name="description">The discount description.</param>
    /// <param name="startingDate">The discount starting date.</param>
    /// <param name="endingDate">The discount ending date.</param>
    /// <returns>A new instance of the <see cref="Discount"/> class.</returns>
    public static Discount CreateDiscount(
        int? percentage = null,
        string? description = null,
        DateTimeOffset? startingDate = null,
        DateTimeOffset? endingDate = null
    )
    {
        return Discount.Create(
            percentage ?? DomainConstants.Discount.Percentage,
            description ?? DomainConstants.Discount.Description,
            startingDate ?? DomainConstants.Discount.StartingDate,
            endingDate ?? DomainConstants.Discount.EndingDate
        );
    }
}

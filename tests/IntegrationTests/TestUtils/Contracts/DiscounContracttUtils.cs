using Contracts.Common;

namespace IntegrationTests.TestUtils.Contracts;

/// <summary>
/// Utilities for the <see cref="DiscountContract"/> contract object.
/// </summary>
public static class DiscounContracttUtils
{

    /// <summary>
    /// Creates a new instance of the <see cref="DiscountContract"/> class.
    /// </summary>
    /// <param name="percentage">The discount percentage.</param>
    /// <param name="description">The discount description.</param>
    /// <param name="startingDate">The discount starting date.</param>
    /// <param name="endingDate">The discount ending date.</param>
    /// <returns>A new instance of the <see cref="DiscountContract"/> class.</returns>
    public static DiscountContract CreateDiscount(
        int percentage = 10,
        string description = "Ten percent discount",
        DateTimeOffset? startingDate = null,
        DateTimeOffset? endingDate = null
    )
    {
        var now = DateTimeOffset.UtcNow;

        return new DiscountContract(
            percentage,
            description,
            startingDate ?? now,
            endingDate ?? now.AddDays(1)
        );
    }
}

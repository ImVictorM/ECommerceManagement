using SharedKernel.ValueObjects;

using Bogus;

namespace SharedKernel.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Discount"/> class.
/// </summary>
public static class DiscountUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="Discount"/> class.
    /// </summary>
    /// <param name="percentage">The discount percentage.</param>
    /// <param name="description">The discount description.</param>
    /// <param name="startingDate">The discount starting date.</param>
    /// <param name="endingDate">The discount ending date.</param>
    /// <returns>A new instance of the <see cref="Discount"/> class.</returns>
    public static Discount CreateDiscount(
        Percentage? percentage = null,
        string? description = null,
        DateTimeOffset? startingDate = null,
        DateTimeOffset? endingDate = null
    )
    {
        var startingDateDiscount = startingDate ?? _faker.Date.BetweenOffset(
            DateTimeOffset.UtcNow.AddHours(-5),
            DateTimeOffset.UtcNow.AddMonths(1)
        );

        var endingDateDiscount = endingDate ?? _faker.Date.BetweenOffset(
            startingDateDiscount.AddHours(2),
            startingDateDiscount.AddMonths(1)
        );

        return Discount.Create(
            percentage ?? PercentageUtils.Create(),
            description ?? _faker.Commerce.ProductName(),
            startingDateDiscount,
            endingDateDiscount
        );
    }

    /// <summary>
    /// Creates a list of discounts.
    /// </summary>
    /// <param name="count">The number of discounts to be created.</param>
    /// <returns>A list of discounts.</returns>
    public static IEnumerable<Discount> CreateDiscounts(int count = 1)
    {
        return Enumerable
            .Range(0, count)
            .Select(index => CreateDiscount());
    }
}

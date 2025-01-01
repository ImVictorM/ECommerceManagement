using Contracts.Common;

using Bogus;

namespace IntegrationTests.TestUtils.Contracts;

/// <summary>
/// Utilities for the <see cref="DiscountContract"/> contract object.
/// </summary>
public static class DiscountContractUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="DiscountContract"/> class.
    /// </summary>
    /// <param name="percentage">The discount percentage.</param>
    /// <param name="description">The discount description.</param>
    /// <param name="startingDate">The discount starting date.</param>
    /// <param name="endingDate">The discount ending date.</param>
    /// <returns>A new instance of the <see cref="DiscountContract"/> class.</returns>
    public static DiscountContract CreateDiscount(
        int? percentage = null,
        string description = "Ten percent discount",
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

        return new DiscountContract(
            percentage ?? _faker.Random.Int(5, 10),
            description ?? _faker.Lorem.Sentence(),
            startingDate ?? startingDateDiscount,
            endingDate ?? endingDateDiscount
        );

    }
}

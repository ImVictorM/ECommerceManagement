using SharedKernel.ValueObjects;
using SharedKernel.UnitTests.TestUtils.Constants;

namespace SharedKernel.UnitTests.TestUtils;

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
        Percentage? percentage = null,
        string? description = null,
        DateTimeOffset? startingDate = null,
        DateTimeOffset? endingDate = null
    )
    {
        return Discount.Create(
            percentage ?? PercentageUtils.Create(SharedKernelConstants.Discount.DiscountPercentage),
            description ?? SharedKernelConstants.Discount.Description,
            startingDate ?? SharedKernelConstants.Discount.StartingDate,
            endingDate ?? SharedKernelConstants.Discount.EndingDate
        );
    }

    /// <summary>
    /// Creates a list of discounts.
    /// </summary>
    /// <param name="count">The discounts number to be created.</param>
    /// <returns>A list of discounts</returns>
    public static IEnumerable<Discount> CreateDiscounts(
        int count = 1
    )
    {
        return Enumerable
            .Range(0, count)
            .Select(index => CreateDiscount(description: CreateDescriptionFromIndex(index)));
    }

    /// <summary>
    /// Creates a new unique description based on an index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>A new unique description.</returns>
    public static string CreateDescriptionFromIndex(int index)
    {
        return $"{SharedKernelConstants.Discount.Description}-{index}";
    }
}

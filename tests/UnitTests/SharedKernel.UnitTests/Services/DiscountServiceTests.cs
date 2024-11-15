using FluentAssertions;
using SharedKernel.Services;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

namespace SharedKernel.UnitTests.Services;

/// <summary>
/// Unit tests for the <see cref="DiscountService"/> service.
/// </summary>
public class DiscountServiceTests
{
    /// <summary>
    /// List containing base price, discounts, and the expected total after discounts were applied.
    /// </summary>
    public static IEnumerable<object[]> DiscountsWithExpectedTotalAfterDiscounts()
    {
        yield return new object[]
        {
            100,
            new List<Discount>()
            {
                DiscountUtils.CreateDiscount(percentage: 10)
            }.ToArray(),
            90,
        };

        yield return new object[]
        {
            100,
            new List<Discount>()
            {
                DiscountUtils.CreateDiscount(percentage: 10),
                DiscountUtils.CreateDiscount(percentage: 10)
            }.ToArray(),
            81,
        };

        yield return new object[]
        {
            300,
            new List<Discount>()
            {
                DiscountUtils.CreateDiscount(percentage: 10),
                DiscountUtils.CreateDiscount(percentage: 50)
            }.ToArray(),
            135,
        };
    }

    /// <summary>
    /// Tests the service calculates the total applying discounts correctly.
    /// </summary>
    /// <param name="basePrice">The base price.</param>
    /// <param name="discounts">The discounts.</param>
    /// <param name="expectedTotalWithDiscountsApplied">The expected total.</param>
    [Theory]
    [MemberData(nameof(DiscountsWithExpectedTotalAfterDiscounts))]
    public void DiscountService_WhenApplyingDiscounts_CalculatesTotalCorrectlyAndReturnsIt(
        decimal basePrice,
        Discount[] discounts,
        decimal expectedTotalWithDiscountsApplied
    )
    {
        var total = DiscountService.ApplyDiscounts(basePrice, discounts);

        total.Should().Be(expectedTotalWithDiscountsApplied);
    }
}

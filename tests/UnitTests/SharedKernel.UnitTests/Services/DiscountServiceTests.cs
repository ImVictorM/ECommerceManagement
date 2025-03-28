using SharedKernel.Services;
using SharedKernel.UnitTests.TestUtils;
using SharedKernel.ValueObjects;

using FluentAssertions;

namespace SharedKernel.UnitTests.Services;

/// <summary>
/// Unit tests for the <see cref="DiscountService"/> service.
/// </summary>
public class DiscountServiceTests
{
    private readonly DiscountService _service;

    /// <summary>
    /// Initiates a new instance of the <see cref="DiscountServiceTests"/> class.
    /// </summary>
    public DiscountServiceTests()
    {
        _service = new DiscountService();
    }

    /// <summary>
    /// Defines a list containing base price, discounts, and the expected total 
    /// after discounts were applied.
    /// </summary>
    public static IEnumerable<object[]> DiscountsWithExpectedTotalAfterDiscounts()
    {
        yield return new object[]
        {
            100,
            new List<Discount>()
            {
                DiscountUtils.CreateDiscount(percentage: PercentageUtils.Create(10))
            }.ToArray(),
            90,
        };

        yield return new object[]
        {
            100,
            new List<Discount>()
            {
                DiscountUtils.CreateDiscount(percentage: PercentageUtils.Create(10)),
                DiscountUtils.CreateDiscount(percentage: PercentageUtils.Create(10))
            }.ToArray(),
            81,
        };

        yield return new object[]
        {
            300,
            new List<Discount>()
            {
                DiscountUtils.CreateDiscount(percentage: PercentageUtils.Create(10)),
                DiscountUtils.CreateDiscount(percentage: PercentageUtils.Create(50))
            }.ToArray(),
            135,
        };
    }

    /// <summary>
    /// Verifies the service calculates the total applying discounts correctly.
    /// </summary>
    /// <param name="basePrice">The base price.</param>
    /// <param name="discounts">The discounts.</param>
    /// <param name="expectedTotalWithDiscountsApplied">
    /// The expected total.
    /// </param>
    [Theory]
    [MemberData(nameof(DiscountsWithExpectedTotalAfterDiscounts))]
    public void CalculateDiscountedPrice_WithValidDiscounts_ReturnsCalculatedTotal(
        decimal basePrice,
        Discount[] discounts,
        decimal expectedTotalWithDiscountsApplied
    )
    {
        var total = _service.CalculateDiscountedPrice(basePrice, discounts);

        total.Should().Be(expectedTotalWithDiscountsApplied);
    }
}

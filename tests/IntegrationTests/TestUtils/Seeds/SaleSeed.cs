using Domain.UnitTests.TestUtils;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;
using Domain.ProductAggregate;
using SharedKernel.Services;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// The available sales in the database seed.
/// </summary>
public enum SeedAvailableSales
{
    /// <summary>
    /// Represents a computer sale.
    /// </summary>
    COMPUTER_SALE
}

/// <summary>
/// Contain sale seed data.
/// </summary>
public static class SaleSeed
{
    private static readonly Dictionary<SeedAvailableSales, Sale> _sales = new()
    {
        [SeedAvailableSales.COMPUTER_SALE] = SaleUtils.CreateSale
        (
            id: SaleId.Create(-1),
            discount: DiscountUtils.CreateDiscount(
                percentage: PercentageUtils.Create(10),
                description: "computer discount",
                startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                endingDate: DateTimeOffset.UtcNow.AddDays(5)
            ),
            categoriesInSale: new HashSet<CategoryReference>() { },
            productsInSale: new HashSet<ProductReference>()
            {
                ProductReference.Create(ProductSeed.GetSeedProduct(SeedAvailableProducts.COMPUTER_ON_SALE).Id)
            },
            productsExcludeFromSale: new HashSet<ProductReference>()
        )
    };

    /// <summary>
    /// List all the seed sales.
    /// </summary>
    public static IReadOnlyList<Sale> ListSales()
    {
        return [.. _sales.Values];
    }

    /// <summary>
    /// Retrieves a seed sale by type.
    /// </summary>
    /// <param name="saleType">The sale type.</param>
    /// <returns>The sale.</returns>
    public static Sale GetSeedSale(SeedAvailableSales saleType)
    {
        return _sales[saleType];
    }

    /// <summary>
    /// Calculates the expected product price after applying the product sales.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>The total applying sales.</returns>
    public static decimal CalculateExpectedPriceAfterApplyingSales(Product product)
    {
        var productCategoryIds = product.ProductCategories.Select(c => c.CategoryId).ToHashSet();

        var saleProduct = SaleProduct.Create(product.Id, productCategoryIds);

        var discounts = _sales.Values.Where(s => s.IsProductInSale(saleProduct)).Select(s => s.Discount);

        return DiscountService.ApplyDiscounts(product.BasePrice, discounts.ToArray());
    }
}

using Domain.UnitTests.TestUtils;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;
using Domain.ProductAggregate;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.Services;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;

namespace IntegrationTests.Common.Seeds.Sales;

/// <summary>
/// Provides seed data for sales in the database.
/// </summary>
public sealed class SaleSeed : DataSeed<SaleSeedType, Sale>
{
    /// <inheritdoc/>
    public override int Order { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="SaleSeed"/> class.
    /// </summary>
    /// <param name="productSeed">The product seed.</param>
    public SaleSeed(IDataSeed<ProductSeedType, Product> productSeed) : base(CreateSeedData(productSeed))
    {
        Order = productSeed.Order + 20;
    }

    private static Dictionary<SaleSeedType, Sale> CreateSeedData(IDataSeed<ProductSeedType, Product> productSeed)
    {
        return new()
        {
            [SaleSeedType.COMPUTER_SALE] = SaleUtils.CreateSale
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
                    ProductReference.Create(productSeed.GetByType(ProductSeedType.COMPUTER_ON_SALE).Id)
                },
                productsExcludeFromSale: new HashSet<ProductReference>()
            )
        };
    }

    /// <summary>
    /// Calculates the expected product price after applying the product sales.
    /// </summary>
    /// <param name="product">The product.</param>
    /// <returns>The total applying sales.</returns>
    public decimal CalculateExpectedPriceAfterApplyingSales(Product product)
    {
        var productCategoryIds = product.ProductCategories.Select(c => c.CategoryId).ToHashSet();

        var saleProduct = SaleProduct.Create(product.Id, productCategoryIds);

        var discounts = Data.Values.Where(s => s.IsProductInSale(saleProduct)).Select(s => s.Discount);

        return DiscountService.ApplyDiscounts(product.BasePrice, discounts.ToArray());
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(ECommerceDbContext context)
    {
        await context.Sales.AddRangeAsync(ListAll());
    }
}

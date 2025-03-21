using Domain.UnitTests.TestUtils;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;
using IntegrationTests.Common.Seeds.Categories;

namespace IntegrationTests.Common.Seeds.Sales;

/// <summary>
/// Provides seed data for sales in the database.
/// </summary>
public sealed class SaleSeed : DataSeed<SaleSeedType, Sale, SaleId>, ISaleSeed
{
    /// <inheritdoc/>
    public override int Order { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="SaleSeed"/> class.
    /// </summary>
    /// <param name="productSeed">The product seed.</param>
    /// <param name="categorySeed">The category seed.</param>
    public SaleSeed(IProductSeed productSeed, ICategorySeed categorySeed)
        : base(CreateSeedData(productSeed, categorySeed))
    {
        Order = productSeed.Order + categorySeed.Order + 20;
    }

    private static Dictionary<SaleSeedType, Sale> CreateSeedData(
        IProductSeed productSeed,
        ICategorySeed categorySeed
    )
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
                categoriesOnSale: [],
                productsOnSale:
                [
                    SaleProduct.Create(productSeed.GetEntityId(
                        ProductSeedType.COMPUTER_ON_SALE
                    ))
                ],
                productsExcludedFromSale: []
            ),
            [SaleSeedType.TECH_SALE] = SaleUtils.CreateSale
            (
                id: SaleId.Create(-2),
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(5),
                    description: "tech discount",
                    startingDate: DateTimeOffset.UtcNow.AddHours(-5),
                    endingDate: DateTimeOffset.UtcNow.AddMonths(2)
                ),
                categoriesOnSale: [
                    SaleCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.TECHNOLOGY
                    ))
                ],
                productsOnSale: [],
                productsExcludedFromSale: []
            ),
            [SaleSeedType.SPORTS_COMING_SALE] = SaleUtils.CreateSale
            (
                id: SaleId.Create(-3),
                discount: DiscountUtils.CreateDiscount(
                    percentage: PercentageUtils.Create(7),
                    description: "sports discount",
                    startingDate: DateTimeOffset.UtcNow.AddDays(6),
                    endingDate: DateTimeOffset.UtcNow.AddDays(13)
                ),
                categoriesOnSale: [
                    SaleCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.SPORTS
                    ))
                ],
                productsOnSale: [],
                productsExcludedFromSale: []
            ),
        };
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await context.Sales.AddRangeAsync(ListAll());
    }
}

using Domain.UnitTests.TestUtils;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Products;

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
    public SaleSeed(IProductSeed productSeed)
        : base(CreateSeedData(productSeed))
    {
        Order = productSeed.Order + 20;
    }

    private static Dictionary<SaleSeedType, Sale> CreateSeedData(
        IProductSeed productSeed
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
            )
        };
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await context.Sales.AddRangeAsync(ListAll());
    }
}

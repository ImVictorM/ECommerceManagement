using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.Factories;
using Domain.SaleAggregate.Services;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

using Moq;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Sale"/> class.
/// </summary>
public static class SaleUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Sale"/> class.
    /// </summary>
    /// <param name="id">The sale id.</param>
    /// <param name="discount">The sale discount percentage.</param>
    /// <param name="categoriesOnSale">The categories on sale.</param>
    /// <param name="productsOnSale">The products on sale.</param>
    /// <param name="productsExcludedFromSale">The products excluded from sale.</param>
    /// <returns>A new instance of the <see cref="Sale"/> class.</returns>
    public static async Task<Sale> CreateSaleAsync(
        SaleId? id = null,
        Discount? discount = null,
        IEnumerable<SaleCategory>? categoriesOnSale = null,
        IEnumerable<SaleProduct>? productsOnSale = null,
        IEnumerable<SaleProduct>? productsExcludedFromSale = null
    )
    {
        var factory = CreateFactoryMock();

        var sale = await factory.CreateSaleAsync(
            discount ?? DiscountUtils.CreateDiscountValidToDate(),
            categoriesOnSale ??
            [
                SaleCategory.Create(CategoryId.Create(1))
            ],
            productsOnSale ??
            [
                SaleProduct.Create(ProductId.Create(1))
            ],
            productsExcludedFromSale ??
            [
                SaleProduct.Create(ProductId.Create(2))
            ]
        );

        if (id != null)
        {
            sale.SetIdUsingReflection(id);
        }

        return sale;
    }

    private static SaleFactory CreateFactoryMock()
    {
        var mockEligibilityService = new Mock<ISaleEligibilityService>();

        mockEligibilityService
            .Setup(s => s.IsSaleEligibleAsync(
                It.IsAny<Sale>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(true);

        return new SaleFactory(mockEligibilityService.Object);
    }
}

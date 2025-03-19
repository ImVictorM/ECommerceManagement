using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils.Extensions;
using SharedKernel.ValueObjects;

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
    public static Sale CreateSale(
        SaleId? id = null,
        Discount? discount = null,
        IEnumerable<SaleCategory>? categoriesOnSale = null,
        IEnumerable<SaleProduct>? productsOnSale = null,
        IEnumerable<SaleProduct>? productsExcludedFromSale = null
    )
    {
        var sale = Sale.Create(
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
}

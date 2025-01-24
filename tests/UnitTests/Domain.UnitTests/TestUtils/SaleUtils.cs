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
    /// <param name="categoriesInSale">The categories in sale.</param>
    /// <param name="productsInSale">The products in sale.</param>
    /// <param name="productsExcludeFromSale">The products excluded from sale.</param>
    /// <returns>A new instance of the <see cref="Sale"/> class.</returns>
    public static Sale CreateSale(
        SaleId? id = null,
        Discount? discount = null,
        IReadOnlySet<CategoryReference>? categoriesInSale = null,
        IReadOnlySet<ProductReference>? productsInSale = null,
        IReadOnlySet<ProductReference>? productsExcludeFromSale = null
    )
    {
        var sale = Sale.Create(
            discount ?? DiscountUtils.CreateDiscount(),
            categoriesInSale ?? new HashSet<CategoryReference>()
            {
                CategoryReference.Create(CategoryId.Create(1))
            },
            productsInSale ?? new HashSet<ProductReference>()
            {
                ProductReference.Create(ProductId.Create(1))
            },
            productsExcludeFromSale ?? new HashSet<ProductReference>()
            {
                ProductReference.Create(ProductId.Create(2))
            }
        );

        if (id != null)
        {
            sale.SetIdUsingReflection(id);
        }

        return sale;
    }
}

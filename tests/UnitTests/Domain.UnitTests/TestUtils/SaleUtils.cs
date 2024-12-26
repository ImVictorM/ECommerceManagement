using Domain.SaleAggregate;
using Domain.SaleAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;
using SharedKernel.UnitTests.TestUtils;
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
    /// <param name="discount">The sale discount percentage.</param>
    /// <param name="categoriesInSale">The categories in sale.</param>
    /// <param name="productsInSale">The products in sale.</param>
    /// <param name="productsExcludeFromSale">The products excluded from sale.</param>
    /// <returns>A new instance of the <see cref="Sale"/> class.</returns>
    public static Sale CreateSale(
        Discount? discount = null,
        IReadOnlySet<CategoryReference>? categoriesInSale = null,
        IReadOnlySet<ProductReference>? productsInSale = null,
        IReadOnlySet<ProductReference>? productsExcludeFromSale = null
    )
    {
        return Sale.Create(
            discount ?? DiscountUtils.CreateDiscount(PercentageUtils.Create(DomainConstants.Sale.DiscountPercentage)),
            categoriesInSale ?? DomainConstants.Sale.CategoriesInSale,
            productsInSale ?? DomainConstants.Sale.ProductsInSale,
            productsExcludeFromSale ?? DomainConstants.Sale.ProductsExcludedFromSale
        );
    }
}

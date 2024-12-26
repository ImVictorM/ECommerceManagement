using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;
using Domain.SaleAggregate.ValueObjects;

namespace Domain.UnitTests.TestUtils.Constants;

public static partial class DomainConstants
{
    /// <summary>
    /// Defines constants for a sale object.
    /// </summary>
    public static class Sale
    {
        /// <summary>
        /// Defines the sale discount percentage.
        /// </summary>
        public const int DiscountPercentage = 10;

        /// <summary>
        /// Defines the categories in sale.
        /// </summary>
        public static readonly IReadOnlySet<CategoryReference> CategoriesInSale = new HashSet<CategoryReference>()
        {
            CategoryReference.Create(CategoryId.Create(1)),
            CategoryReference.Create(CategoryId.Create(2)),
        };

        /// <summary>
        /// Defines the products in sale.
        /// </summary>
        public static readonly IReadOnlySet<ProductReference> ProductsInSale = new HashSet<ProductReference>()
        {
            ProductReference.Create(ProductId.Create(1)),
            ProductReference.Create(ProductId.Create(2)),
        };

        /// <summary>
        /// Defines the products excluded from sale.
        /// </summary>
        public static readonly IReadOnlySet<ProductReference> ProductsExcludedFromSale = new HashSet<ProductReference>()
        {
            ProductReference.Create(ProductId.Create(2)),
        };
    }
}

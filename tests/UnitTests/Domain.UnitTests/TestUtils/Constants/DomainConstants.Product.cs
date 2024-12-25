using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.UnitTests.TestUtils.Constants;

/// <summary>
/// Defines domain constants.
/// </summary>
public static partial class DomainConstants
{
    /// <summary>
    /// Product constants for testing purposes.
    /// </summary>
    public static class Product
    {
        /// <summary>
        /// The product id constant.
        /// </summary>
        public static readonly ProductId Id = ProductId.Create(1);
        /// <summary>
        /// The product name constant.
        /// </summary>
        public const string Name = "Pen";
        /// <summary>
        /// The product description constant.
        /// </summary>
        public const string Description = "A normal pen";
        /// <summary>
        /// The product price constant.
        /// </summary>
        public const decimal BasePrice = 1.50m;
        /// <summary>
        /// The product quantity available in inventory constant.
        /// </summary>
        public const int QuantityInInventory = 10;

        /// <summary>
        /// The product category constant.
        /// </summary>
        public static readonly IReadOnlyList<ProductCategory> Categories =
        [
            ProductCategory.Create(CategoryId.Create(1)),
            ProductCategory.Create(CategoryId.Create(2)),
        ];

        /// <summary>
        /// The product images constant.
        /// </summary>
        public static readonly IReadOnlyList<ProductImage> ProductImages =
        [
            ProductImage.Create(new Uri("https://img.freepik.com/vetores-gratis/caneta-esferografica-escolar-estacionaria_78370-631.jpg"))
        ];
    }
}

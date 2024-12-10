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
        public const decimal Price = 1.50m;
        /// <summary>
        /// The product quantity available in inventory constant.
        /// </summary>
        public const int QuantityAvailable = 10;

        /// <summary>
        /// The product category constant.
        /// </summary>
        public static readonly IEnumerable<string> Categories = ["books_stationery"];

        /// <summary>
        /// The product image constant.
        /// </summary>
        public static readonly Uri ProductImage = new("https://img.freepik.com/vetores-gratis/caneta-esferografica-escolar-estacionaria_78370-631.jpg");

        /// <summary>
        /// Creates a new product image containing the index.
        /// </summary>
        /// <param name="index">The index to be concatenated to the image url.</param>
        /// <returns>A new product image containing the index concatenation.</returns>
        public static Uri ProductImageFromIndex(int index)
        {
            return new Uri($"{ProductImage}-{index}");
        }

        /// <summary>
        /// Creates a new product name concatenating the index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A new products name.</returns>
        public static string ProductNameFromIndex(int index)
        {
            return $"{Name}-{index}";
        }

        /// <summary>
        /// Creates a new product description concatenating the index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>A new product description.</returns>
        public static string ProductDescriptionFromIndex(int index)
        {
            return $"{Description}-{index}";
        }

        /// <summary>
        /// Defines product validation error message constants.
        /// </summary>
        public static class Validations
        {
            /// <summary>
            /// Error message for empty names.
            /// </summary>
            public const string EmptyName = "'Name' must not be empty.";
            /// <summary>
            /// Error message for empty descriptions.
            /// </summary>
            public const string EmptyDescription = "'Description' must not be empty.";
            /// <summary>
            /// Error message for empty categories.
            /// </summary>
            public const string EmptyCategories = "'Categories' must not be empty.";
            /// <summary>
            /// Error message for empty images.
            /// </summary>
            public const string EmptyImages = "'Images' must not be empty.";
            /// <summary>
            /// Error message for less than zero initial quantities.
            /// </summary>
            public const string LessThanZeroInitialQuantity = "'Initial Quantity' must be greater than '0'.";
            /// <summary>
            /// Error message for less than zero initial prices.
            /// </summary>
            public const string LessThanZeroBasePrice = "'Base Price' must be greater than '0'.";
            /// <summary>
            /// Error message for less than zero quantity to increment.
            /// </summary>
            public const string LessThanZeroQuantityToIncrement = "'Quantity To Increment' must be greater than '0'.";
        }
    }
}

using Domain.ProductAggregate;
using Domain.ProductAggregate.Enumerations;
using Domain.UnitTests.TestUtils;
using SharedKernel.UnitTests.TestUtils;

namespace IntegrationTests.TestUtils.Seeds;

/// <summary>
/// The available product types in the database seed.
/// </summary>
public enum SeedAvailableProducts
{
    /// <summary>
    /// Simple computer containing discounts.
    /// </summary>
    COMPUTER
}

/// <summary>
/// Contain product seed data.
/// </summary>
public static class ProductSeed
{
    /// <summary>
    /// The seed products.
    /// </summary>
    private static readonly Dictionary<SeedAvailableProducts, Product> _products = new Dictionary<SeedAvailableProducts, Product>()
    {
        [SeedAvailableProducts.COMPUTER] = ProductUtils.CreateProduct(
            name: "Computer",
            description: "Simple computer",
            price: 3000m,
            quantityAvailable: 50,
            categories: [
                Category.Electronics.Name,
                Category.OfficeSupplies.Name
            ],
            productImagesUrl: [
                new Uri("computer-thumb.png", UriKind.Relative),
                new Uri("computer-left-side.png", UriKind.Relative),
                new Uri("computer-right-side.png", UriKind.Relative)
            ],
            initialDiscounts: [
                DiscountUtils.CreateDiscount(percentage: 20, description: "Black Friday discount"),
                DiscountUtils.CreateDiscount(percentage: 5, description: "Base discount")
            ]
        )
    };

    /// <summary>
    /// List all the seed products.
    /// </summary>
    public static IEnumerable<Product> ListProducts()
    {
        return _products.Values;
    }

    /// <summary>
    /// Retrieves a seed product by type.
    /// </summary>
    /// <param name="productType">The product type.</param>
    /// <returns>The product.</returns>
    public static Product GetSeedProduct(SeedAvailableProducts productType)
    {
        return _products[productType];
    }
}

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
    COMPUTER_WITH_DISCOUNTS,
    /// <summary>
    /// Represents a t-shirt.
    /// </summary>
    TSHIRT,
    /// <summary>
    /// Represents a chain bracelet.
    /// </summary>
    CHAIN_BRACELET,
    /// <summary>
    /// Represents a pencil.
    /// </summary>
    PENCIL
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
        [SeedAvailableProducts.COMPUTER_WITH_DISCOUNTS] = ProductUtils.CreateProduct(
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
                DiscountUtils.CreateDiscount(percentage: 5, description: "Base discount"),
                DiscountUtils.CreateDiscount(
                    percentage: 30,
                    description: "Future discount",
                    startingDate: DateTimeOffset.UtcNow.AddDays(2),
                    endingDate: DateTimeOffset.UtcNow.AddDays(5)
                ),
                DiscountUtils.CreateDiscount(
                    percentage: 10,
                    description: "Next month discount",
                    startingDate: DateTimeOffset.UtcNow.AddMonths(1),
                    endingDate: DateTimeOffset.UtcNow.AddMonths(2)
                )
            ]
        ),
        [SeedAvailableProducts.TSHIRT] = ProductUtils.CreateProduct(
            name: "Mens Casual Premium Slim Fit T-Shirts",
            description: "Slim-fitting style, contrast raglan long sleeve, three-button henley placket, light weight & soft fabric for breathable and comfortable wearing.",
            price: 22.3m,
            quantityAvailable: 10,
            categories: [
                Category.Fashion.Name,
                Category.SportsOutdoor.Name
            ],
            productImagesUrl: [
                new Uri("t-shirt.png", UriKind.Relative)
            ]
        ),
        [SeedAvailableProducts.CHAIN_BRACELET] = ProductUtils.CreateProduct(
            name: "John Hardy Women's Legends Naga Gold & Silver Dragon Station Chain Bracelet",
            description: "From our Legends Collection, the Naga was inspired by the mythical water dragon that protects the ocean's pearl.",
            price: 695m,
            quantityAvailable: 2,
            categories: [
                Category.JewelryWatches.Name,
                Category.Fashion.Name
            ],
            productImagesUrl: [
                new Uri("bracelet.png", UriKind.Relative)
            ]
        ),
        [SeedAvailableProducts.PENCIL] = ProductUtils.CreateProduct(
            name: "Holiday Mixed Set - Blackwing Matte - Set of 12",
            description: "Ideal for artists, musicians, woodworkers, and anyone who prefers a soft, dark line.",
            price: 160m,
            quantityAvailable: 150,
            categories: [
                Category.BooksStationery.Name
            ],
            productImagesUrl: [
                new Uri("pencil.png", UriKind.Relative)
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

    /// <summary>
    /// Gets the products that contains certain categories.
    /// </summary>
    /// <param name="categories">The categories the products should contain.</param>
    /// <returns>A list of products that contain certain categories.</returns>
    public static IEnumerable<Product> GetSeedProductsByCategories(params Category[] categories)
    {
        var categoriesId = categories.Select(c => c.Id);

        return ListProducts()
            .Where(p =>
                p.ProductCategories
                    .Select(pc => pc.CategoryId)
                    .Any(id => categoriesId.Contains(id))
            );
    }
}

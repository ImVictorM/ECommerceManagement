using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

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
    PENCIL,
    /// <summary>
    /// Represents a jacket that is inactive.
    /// </summary>
    INACTIVE_JACKET
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
                ProductCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.TECHNOLOGY).Id),
                ProductCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.OFFICE_SUPPLIES).Id)
            ],
            images: [

                ProductImage.Create(new Uri("computer-thumb.png", UriKind.Relative)),
                ProductImage.Create(new Uri("computer-left-side.png", UriKind.Relative)),
                ProductImage.Create(new Uri("computer-right-side.png", UriKind.Relative))
            ]
        ),
        [SeedAvailableProducts.TSHIRT] = ProductUtils.CreateProduct(
            name: "Mens Casual Premium Slim Fit T-Shirts",
            description: "Slim-fitting style, contrast raglan long sleeve, three-button henley placket, light weight & soft fabric for breathable and comfortable wearing.",
            price: 22.3m,
            quantityAvailable: 10,
            categories: [
                ProductCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.FASHION).Id),
                ProductCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.SPORTS).Id)
            ],
            images: [
                ProductImage.Create(new Uri("t-shirt.png", UriKind.Relative))
            ]
        ),
        [SeedAvailableProducts.CHAIN_BRACELET] = ProductUtils.CreateProduct(
            name: "John Hardy Women's Legends Naga Gold & Silver Dragon Station Chain Bracelet",
            description: "From our Legends Collection, the Naga was inspired by the mythical water dragon that protects the ocean's pearl.",
            price: 695m,
            quantityAvailable: 2,
            categories: [
                ProductCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.JEWELRY).Id),
                ProductCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.FASHION).Id)
            ],
            images: [
                ProductImage.Create(new Uri("bracelet.png", UriKind.Relative))
            ]
        ),
        [SeedAvailableProducts.PENCIL] = ProductUtils.CreateProduct(
            name: "Holiday Mixed Set - Blackwing Matte - Set of 12",
            description: "Ideal for artists, musicians, woodworkers, and anyone who prefers a soft, dark line.",
            price: 160m,
            quantityAvailable: 150,
            categories: [
                ProductCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.BOOKS_STATIONERY).Id),
            ],
            images: [
                ProductImage.Create(new Uri("pencil.png", UriKind.Relative)),
            ]
        ),
        [SeedAvailableProducts.INACTIVE_JACKET] = ProductUtils.CreateProduct(
            name: "Mens Cotton Jacket",
            description: "Great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions.",
            price: 200,
            categories: [
                ProductCategory.Create(CategorySeed.GetSeedCategory(SeedAvailableCategories.FASHION).Id),
            ],
            images: [
                ProductImage.Create(new Uri("jacket.png", UriKind.Relative))
            ],
            active: false
        )
    };

    /// <summary>
    /// List all the seed products.
    /// </summary>
    /// <param name="filter">A filter to list specific products.</param>
    public static IEnumerable<Product> ListProducts(Func<Product, bool>? filter = null)
    {
        return filter != null ? _products.Values.Where(filter) : _products.Values;
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
    /// <param name="categoryIds">The categories the products should contain.</param>
    /// <returns>A list of products that contain certain categories.</returns>
    public static IEnumerable<Product> GetSeedProductsByCategories(params CategoryId[] categoryIds)
    {
        return ListProducts()
            .Where(p =>
                p.ProductCategories
                    .Select(pc => pc.CategoryId)
                    .Any(id => categoryIds.Contains(id))
            );
    }
}

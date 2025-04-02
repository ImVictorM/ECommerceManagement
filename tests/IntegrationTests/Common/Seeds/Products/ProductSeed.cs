using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils;

using Infrastructure.Common.Persistence;

using IntegrationTests.Common.Seeds.Abstracts;
using IntegrationTests.Common.Seeds.Categories;

namespace IntegrationTests.Common.Seeds.Products;

/// <summary>
/// Provides seed data for products in the database.
/// </summary>
public sealed class ProductSeed
    : DataSeed<ProductSeedType, Product, ProductId>, IProductSeed
{
    /// <inheritdoc/>
    public override int Order { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductSeed"/> class.
    /// </summary>
    /// <param name="categorySeed">The category seed.</param>
    public ProductSeed(ICategorySeed categorySeed)
        : base(CreateSeedData(categorySeed))
    {
        Order = categorySeed.Order + 20;
    }

    private static Dictionary<ProductSeedType, Product> CreateSeedData(
        ICategorySeed categorySeed
    )
    {
        return new Dictionary<ProductSeedType, Product>()
        {
            [ProductSeedType.COMPUTER_ON_SALE] = ProductUtils.CreateProduct(
                id: ProductId.Create(-1),
                name: "Computer",
                description: "Simple computer",
                basePrice: 3000m,
                initialQuantityInInventory: 50,
                categories: [
                    ProductCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.TECHNOLOGY
                    )),
                    ProductCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.OFFICE_SUPPLIES
                    ))
                ],
                images: [

                    ProductImage.Create(new Uri(
                        "computer-thumb.png",
                        UriKind.Relative
                    )),
                    ProductImage.Create(new Uri(
                        "computer-left-side.png",
                        UriKind.Relative
                    )),
                    ProductImage.Create(new Uri(
                        "computer-right-side.png",
                        UriKind.Relative
                    ))
                ]
            ),
            [ProductSeedType.TSHIRT] = ProductUtils.CreateProduct(
                id: ProductId.Create(-2),
                name: "Mens Casual Premium Slim Fit T-Shirts",
                description: "Slim-fitting style, contrast raglan long sleeve," +
                " three-button henley placket, light weight & soft fabric for" +
                " breathable and comfortable wearing.",
                basePrice: 22.3m,
                initialQuantityInInventory: 10,
                categories: [
                    ProductCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.FASHION
                    )),
                    ProductCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.SPORTS
                    ))
                ],
                images: [
                    ProductImage.Create(new Uri(
                        "t-shirt.png",
                        UriKind.Relative
                    ))
                ]
            ),
            [ProductSeedType.CHAIN_BRACELET] = ProductUtils.CreateProduct(
                id: ProductId.Create(-3),
                name: "John Hardy Women's Legends Naga Gold & Silver Dragon" +
                " Station Chain Bracelet",
                description: "From our Legends Collection, the Naga was inspired" +
                " by the mythical water dragon that protects the ocean's pearl.",
                basePrice: 695m,
                initialQuantityInInventory: 2,
                categories: [
                    ProductCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.JEWELRY
                    )),
                    ProductCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.FASHION
                    ))
                ],
                images: [
                    ProductImage.Create(new Uri(
                        "bracelet.png",
                        UriKind.Relative
                    ))
                ]
            ),
            [ProductSeedType.PENCIL] = ProductUtils.CreateProduct(
                id: ProductId.Create(-4),
                name: "Holiday Mixed Set - Blackwing Matte - Set of 12",
                description: "Ideal for artists, musicians, woodworkers," +
                " and anyone who prefers a soft, dark line.",
                basePrice: 160m,
                initialQuantityInInventory: 150,
                categories: [
                    ProductCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.BOOKS_STATIONERY
                    )),
                ],
                images: [
                    ProductImage.Create(new Uri(
                        "pencil.png",
                        UriKind.Relative
                    )),
                ]
            ),
            [ProductSeedType.JACKET_INACTIVE] = ProductUtils.CreateProduct(
                id: ProductId.Create(-5),
                name: "Mens Cotton Jacket",
                description: "Great outerwear jackets for Spring/Autumn/Winter," +
                " suitable for many occasions.",
                basePrice: 200,
                categories: [
                    ProductCategory.Create(categorySeed.GetEntityId(
                        CategorySeedType.FASHION
                    )),
                ],
                images: [
                    ProductImage.Create(new Uri(
                        "jacket.png",
                        UriKind.Relative
                    ))
                ],
                active: false
            )
        };
    }

    /// <summary>
    /// Retrieves the products that contains certain categories.
    /// </summary>
    /// <param name="categoryIds">
    /// The categories the products should contain.
    /// </param>
    /// <returns>
    /// A list of products that contain certain categories.
    /// </returns>
    public IReadOnlyList<Product> GetSeedProductsByCategories(
        params CategoryId[] categoryIds
    )
    {
        return ListAll(p =>
            p.ProductCategories
                .Select(pc => pc.CategoryId)
                .Any(id => categoryIds.Contains(id))
        );
    }

    /// <inheritdoc/>
    public override async Task SeedAsync(IECommerceDbContext context)
    {
        await context.Products.AddRangeAsync(ListAll());
    }
}

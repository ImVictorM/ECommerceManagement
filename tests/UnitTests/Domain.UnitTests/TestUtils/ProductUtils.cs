
using Domain.CategoryAggregate.ValueObjects;
using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.UnitTests.TestUtils.Extensions;

using Bogus;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// Utilities for the <see cref="Product"/> class.
/// </summary>
public static class ProductUtils
{
    private static readonly Faker _faker = new();

    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="id">The product id.</param>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="basePrice">The product base price.</param>
    /// <param name="initialQuantityInInventory">The product initial quantity in inventory.</param>
    /// <param name="categories">The product categories.</param>
    /// <param name="images">The product images.</param>
    /// <param name="active">A boolean value indicating the product status.</param>
    /// <returns>A new instance of the <see cref="Product"/> class.</returns>
    public static Product CreateProduct(
        ProductId? id = null,
        string? name = null,
        string? description = null,
        decimal? basePrice = null,
        int? initialQuantityInInventory = null,
        IEnumerable<ProductCategory>? categories = null,
        IEnumerable<ProductImage>? images = null,
        bool active = true
    )
    {
        var product = Product.Create(
            name ?? _faker.Commerce.ProductName(),
            description ?? _faker.Commerce.ProductDescription(),
            basePrice ?? _faker.Random.Decimal(10, 100),
            initialQuantityInInventory ?? _faker.Random.Int(1, 50),
            categories ?? CreateProductCategories(),
            images ?? CreateProductImages()
        );

        if (id != null)
        {
            product.SetIdUsingReflection(id);
        }

        if (!active)
        {
            product.Deactivate();
        }

        return product;
    }

    /// <summary>
    /// Creates a list of <see cref="Product"/> items.
    /// </summary>
    /// <param name="count">The quantity of items to be generated.</param>
    /// <returns>A list containing <see cref="Product"/> items.</returns>
    public static IEnumerable<Product> CreateProducts(int count = 1)
    {
        return Enumerable
            .Range(0, count)
            .Select(index => CreateProduct(id: ProductId.Create(index + 1)));
    }

    /// <summary>
    /// Creates a new list of <see cref="ProductImage"/>.
    /// </summary>
    /// <param name="count">The quantity of items to be generated.</param>
    /// <returns>A list of product images.</returns>
    public static IEnumerable<ProductImage> CreateProductImages(int count = 1)
    {
        var imageURIs = CreateImageURIs(count);

        return imageURIs.Select(ProductImage.Create);
    }

    /// <summary>
    /// Creates a new list of <see cref="ProductCategory"/>.
    /// </summary>
    /// <param name="count">The quantity of items to be generated.</param>
    /// <returns>A list of product categories.</returns>
    public static IEnumerable<ProductCategory> CreateProductCategories(int count = 1)
    {
        var sequence = NumberUtils.CreateNumberSequenceAsString(count);

        return sequence.Select(n => ProductCategory.Create(CategoryId.Create(n)));
    }

    /// <summary>
    /// Creates a new list of image URIs.
    /// </summary>
    /// <param name="count">The quantity of images to be generated.</param>
    /// <returns>A list of image URIs.</returns>
    public static IEnumerable<Uri> CreateImageURIs(int count = 1)
    {
        return Enumerable
            .Range(0, count)
            .Select(index => new Uri(_faker.Internet.UrlRootedPath("png"), UriKind.Relative));
    }
}

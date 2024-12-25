using Domain.ProductAggregate;
using Domain.ProductAggregate.ValueObjects;
using Domain.UnitTests.TestUtils.Constants;

using SharedKernel.UnitTests.TestUtils.Extensions;

namespace Domain.UnitTests.TestUtils;

/// <summary>
/// The product utilities.
/// </summary>
public static class ProductUtils
{
    /// <summary>
    /// Creates a new instance of the <see cref="Product"/> class.
    /// </summary>
    /// <param name="id">The product id.</param>
    /// <param name="name">The product name.</param>
    /// <param name="description">The product description.</param>
    /// <param name="basePrice">The product price.</param>
    /// <param name="initialQuantityInInventory">The product quantity available in inventory.</param>
    /// <param name="categories">The product categories.</param>
    /// <param name="images">The product images.</param>
    /// <param name="active">Defines if the product should be created as an active or inactive product.</param>
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
            name ?? DomainConstants.Product.Name,
            description ?? DomainConstants.Product.Description,
            basePrice ?? DomainConstants.Product.BasePrice,
            initialQuantityInInventory ?? DomainConstants.Product.QuantityInInventory,
            categories ?? DomainConstants.Product.Categories,
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
    /// Creates a list of products.
    /// </summary>
    /// <param name="count">The quantity of products to be created.</param>
    /// <param name="categories">The categories the products will have.</param>
    /// <param name="active">Defines if the products created should be active or inactive.</param>
    /// <returns>A list of products.</returns>
    public static IEnumerable<Product> CreateProducts(
        int count = 1,
        IEnumerable<ProductCategory>? categories = null,
        bool active = true
    )
    {
        return Enumerable
            .Range(0, count)
            .Select(index => CreateProduct(
                name: ProductNameFromIndex(index),
                description: ProductDescriptionFromIndex(index),
                categories: categories,
                active: active
            ));
    }

    /// <summary>
    /// Creates a list of product images.
    /// </summary>
    /// <param name="imageCount">The quantity of product images to be created.</param>
    /// <returns>A list of product images.</returns>
    public static IEnumerable<ProductImage> CreateProductImages(int imageCount = 1)
    {
        return Enumerable
            .Range(0, imageCount)
            .Select(ProductImageUrlFromIndex)
            .Select(ProductImage.Create);
    }

    /// <summary>
    /// Creates a new product image containing the index.
    /// </summary>
    /// <param name="index">The index to be concatenated to the image url.</param>
    /// <returns>A new product image containing the index concatenation.</returns>
    public static Uri ProductImageUrlFromIndex(int index)
    {
        var imageUrl = DomainConstants.Product.ProductImages[0].Url;

        return new Uri($"{imageUrl}-{index}");
    }

    /// <summary>
    /// Creates a new product name concatenating the index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>A new products name.</returns>
    public static string ProductNameFromIndex(int index)
    {
        return $"{DomainConstants.Product.Name}-{index}";
    }

    /// <summary>
    /// Creates a new product description concatenating the index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>A new product description.</returns>
    public static string ProductDescriptionFromIndex(int index)
    {
        return $"{DomainConstants.Product.Description}-{index}";
    }
}

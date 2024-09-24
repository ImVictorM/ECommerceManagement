using Domain.Common.Models;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.ProductAggregate.Entities;

/// <summary>
/// Represents a product image.
/// </summary>
public sealed class ProductImage : Entity<ProductImageId>
{
    /// <summary>
    /// Gets the image url.
    /// </summary>
    public Uri Url { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="ProductImage"/> class.
    /// </summary>
    private ProductImage() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductImage"/> class.
    /// </summary>
    /// <param name="url">The image url.</param>
    private ProductImage(Uri url) : base(ProductImageId.Create())
    {
        Url = url;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductImage"/> class.
    /// </summary>
    /// <param name="url">The image url.</param>
    /// <returns>A new instance of the <see cref="ProductImage"/> class.</returns>
    public static ProductImage Create(Uri url)
    {
        return new ProductImage(url);
    }
}

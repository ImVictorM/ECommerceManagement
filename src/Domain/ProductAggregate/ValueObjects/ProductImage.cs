using SharedKernel.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents a product image.
/// </summary>
public sealed class ProductImage : ValueObject
{
    /// <summary>
    /// Gets the image uri.
    /// </summary>
    public Uri Uri { get; } = null!;

    private ProductImage() { }

    private ProductImage(Uri url)
    {
        Uri = url;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductImage"/> class.
    /// </summary>
    /// <param name="url">The image url.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductImage"/> class.
    /// </returns>
    public static ProductImage Create(Uri url)
    {
        return new ProductImage(url);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Uri;
    }
}

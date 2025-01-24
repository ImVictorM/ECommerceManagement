using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents a sale product.
/// </summary>
public class ProductReference : ValueObject
{
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    private ProductReference() { }

    private ProductReference(ProductId productId)
    {
        ProductId = productId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductReference"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <returns>A new instance of the <see cref="ProductReference"/> class.</returns>
    public static ProductReference Create(ProductId productId)
    {
        return new ProductReference(productId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
    }
}

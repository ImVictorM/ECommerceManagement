using SharedKernel.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents a product reserved.
/// </summary>
public sealed class ProductReserved : ValueObject
{
    /// <summary>
    /// Gets the product reserved product identifier.
    /// </summary>
    public ProductId ProductId { get; } = null!;
    /// <summary>
    /// Gets the quantity reserved.
    /// </summary>
    public int QuantityReserved { get; }

    private ProductReserved() { }

    private ProductReserved(ProductId productId, int quantityReserved)
    {
        ProductId = productId;
        QuantityReserved = quantityReserved;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ProductReserved"/> class.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="quantityReserved">The quantity reserved.</param>
    /// <returns>
    /// A new instance of the <see cref="ProductReserved"/> class.
    /// </returns>
    public static ProductReserved Create(
        ProductId productId,
        int quantityReserved
    )
    {
        return new ProductReserved(productId, quantityReserved);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
        yield return QuantityReserved;
    }
}

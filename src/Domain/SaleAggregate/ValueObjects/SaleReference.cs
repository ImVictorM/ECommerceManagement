using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents a sale product.
/// </summary>
public class SaleReference : ValueObject
{
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    private SaleReference() { }

    private SaleReference(ProductId productId)
    {
        ProductId = productId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SaleReference"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <returns>A new instance of the <see cref="SaleReference"/> class.</returns>
    public static SaleReference Create(ProductId productId)
    {
        return new SaleReference(productId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
    }
}

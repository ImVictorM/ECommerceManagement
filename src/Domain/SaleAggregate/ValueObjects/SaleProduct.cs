using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.SaleAggregate.ValueObjects;

/// <summary>
/// Represents a sale product.
/// </summary>
public class SaleProduct : ValueObject
{
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    private SaleProduct() { }

    private SaleProduct(ProductId productId)
    {
        ProductId = productId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="SaleProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <returns>A new instance of the <see cref="SaleProduct"/> class.</returns>
    public static SaleProduct Create(ProductId productId)
    {
        return new SaleProduct(productId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ProductId;
    }
}

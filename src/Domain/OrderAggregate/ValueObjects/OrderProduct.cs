using Domain.ProductAggregate.ValueObjects;
using SharedKernel.Models;

namespace Domain.OrderAggregate.ValueObjects;

/// <summary>
/// Represents an order product.
/// </summary>
public sealed class OrderProduct : ValueObject
{
    /// <summary>
    /// Gets the quantity of products ordered.
    /// </summary>
    public int Quantity { get; }
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; } = null!;

    private OrderProduct() { }

    private OrderProduct(
        ProductId productId,
        int quantity
    )
    {
        ProductId = productId;
        Quantity = quantity;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="quantity">The quantity of products ordered.</param>
    /// <returns>A new instance of the <see cref="OrderProduct"/> class.</returns>
    public static OrderProduct Create(
        ProductId productId,
        int quantity
    )
    {
        return new OrderProduct(productId, quantity);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Quantity;
        yield return ProductId;
    }
}

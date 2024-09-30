using Domain.Common.Models;
using Domain.OrderAggregate.ValueObjects;
using Domain.ProductAggregate.ValueObjects;

namespace Domain.OrderAggregate.Entities;

/// <summary>
/// Represents an order product.
/// </summary>
public sealed class OrderProduct : Entity<OrderProductId>
{
    /// <summary>
    /// Gets the price when the product was ordered.
    /// </summary>
    public float PriceOnOrder { get; private set; }
    /// <summary>
    /// Gets the quantity of products ordered.
    /// </summary>
    public int Quantity { get; private set; }
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderProduct"/> class.
    /// </summary>
    private OrderProduct() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="priceOnOrder">The product price on order.</param>
    /// <param name="quantity">The quantity of products ordered.</param>
    private OrderProduct(
        ProductId productId,
        float priceOnOrder,
        int quantity
    )
    {
        ProductId = productId;
        PriceOnOrder = priceOnOrder;
        Quantity = quantity;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="OrderProduct"/> class.
    /// </summary>
    /// <param name="productId">The product id.</param>
    /// <param name="priceOnOrder">The product price on order.</param>
    /// <param name="quantity">The quantity of products ordered.</param>
    /// <returns>A new instance of the <see cref="OrderProduct"/> class.</returns>
    public static OrderProduct Create(
        ProductId productId,
        float priceOnOrder,
        int quantity
    )
    {
        return new OrderProduct(productId, priceOnOrder, quantity);
    }
}

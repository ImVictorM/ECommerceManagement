using Domain.ProductAggregate.ValueObjects;

namespace Domain.OrderAggregate.Interfaces;

/// <summary>
/// Represents an order product contract.
/// </summary>
public interface IOrderProductReserved
{
    /// <summary>
    /// Gets the quantity of products ordered.
    /// </summary>
    public int Quantity { get; }
    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; }
}

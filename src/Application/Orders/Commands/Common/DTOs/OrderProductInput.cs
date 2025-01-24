using Domain.OrderAggregate.Interfaces;
using Domain.ProductAggregate.ValueObjects;

namespace Application.Orders.Commands.Common.DTOs;

/// <summary>
/// Represents a product input DTO.
/// </summary>
public class OrderProductInput : IOrderProductReserved
{
    /// <summary>
    /// Gets the quantity.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the product id.
    /// </summary>
    public ProductId ProductId { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="OrderProductInput"/> class.
    /// </summary>
    /// <param name="id">The product id.</param>
    /// <param name="quantity">The product quantity.</param>
    public OrderProductInput(string id, int quantity)
    {
        ProductId = ProductId.Create(id);
        Quantity = quantity;
    }
};

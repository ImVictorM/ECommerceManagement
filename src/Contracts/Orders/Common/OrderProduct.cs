namespace Contracts.Orders.Common;

/// <summary>
/// Represents the product (item) of an order.
/// </summary>
/// <param name="Id">The product id.</param>
/// <param name="Quantity">The quantity to buy of that product.</param>
public record class OrderProduct(
    string Id,
    int Quantity
);

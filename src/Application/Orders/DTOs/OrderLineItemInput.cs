namespace Application.Orders.DTOs;

/// <summary>
/// Represents an order line item input.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="Quantity">The product quantity.</param>
public record OrderLineItemInput(string ProductId, int Quantity);

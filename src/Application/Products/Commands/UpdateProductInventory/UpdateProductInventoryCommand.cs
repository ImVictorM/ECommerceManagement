using MediatR;

namespace Application.Products.Commands.UpdateProductInventory;

/// <summary>
/// Represents a command to update a product's inventory.
/// </summary>
/// <param name="ProductId">The product id.</param>
/// <param name="QuantityToIncrement">The quantity to add to the inventory.</param>
public record UpdateProductInventoryCommand(string ProductId, int QuantityToIncrement) : IRequest<Unit>;

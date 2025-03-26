using Domain.ProductAggregate.Errors;
using Domain.ProductAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.ProductAggregate.Entities;

/// <summary>
/// Represents a product inventory.
/// </summary>
public sealed class Inventory : Entity<InventoryId>
{
    /// <summary>
    /// Gets the quantity available of the related product.
    /// </summary>
    public int QuantityAvailable { get; private set; }

    private Inventory() { }

    private Inventory(int initialQuantity)
    {
        QuantityAvailable = initialQuantity;
    }

    internal static Inventory Create(int quantityAvailable)
    {
        return new Inventory(quantityAvailable);
    }

    /// <summary>
    /// Decreases the quantity available by the value specified.
    /// </summary>
    /// <param name="quantity">The quantity to remove.</param>
    /// <exception cref="InventoryInsufficientException">
    /// Thrown when it is not possible to remove inventory due to insufficient
    /// stock.
    /// </exception>
    public void RemoveStock(int quantity)
    {
        if (!HasSufficientStock(quantity))
        {
            throw new InventoryInsufficientException();
        }

        QuantityAvailable -= quantity;
    }

    /// <summary>
    /// Increments the quantity available by the value specified.
    /// </summary>
    /// <param name="quantity">The quantity to increment.</param>
    public void AddStock(int quantity)
    {
        QuantityAvailable += quantity;
    }

    /// <summary>
    /// Sets the quantity available in inventory to 0.
    /// </summary>
    public void ClearStock()
    {
        QuantityAvailable = 0;
    }

    private bool HasSufficientStock(int quantity)
    {
        return QuantityAvailable > quantity;
    }
}

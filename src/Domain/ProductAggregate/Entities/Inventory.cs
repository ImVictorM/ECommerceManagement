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

    /// <summary>
    /// Initializes a new instance of the <see cref="Inventory"/> class.
    /// </summary>
    private Inventory() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Inventory"/> class.
    /// </summary>
    /// <param name="initialQuantity">The available quantity of the related product.</param>
    private Inventory(int initialQuantity)
    {
        QuantityAvailable = initialQuantity;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Inventory"/> class with the specified quantity available.
    /// </summary>
    /// <param name="quantityAvailable">The available quantity of the related product.</param>
    /// <returns>A new instance of <see cref="Inventory"/>.</returns>
    internal static Inventory Create(int quantityAvailable)
    {
        return new Inventory(quantityAvailable);
    }

    /// <summary>
    /// Decreases the quantity available by the value specified.
    /// </summary>
    /// <param name="quantity">The quantity to remove.</param>
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

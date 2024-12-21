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
    /// <param name="quantityAvailable">The available quantity of the related product.</param>
    private Inventory(int quantityAvailable)
    {
        QuantityAvailable = quantityAvailable;
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
    /// Increments the quantity available by the value specified.
    /// </summary>
    /// <param name="quantityToAdd">The quantity to increment.</param>
    public void IncrementQuantityAvailable(int quantityToAdd)
    {
        QuantityAvailable += quantityToAdd;
    }

    /// <summary>
    /// Verifies the inventory has the quantity available based on the given quantity.
    /// </summary>
    /// <param name="quantity">The quantity to deduct.</param>
    /// <returns>A bool indicating if the inventory has the quantity available.</returns>
    public bool HasInventoryAvailable(int quantity)
    {
        return QuantityAvailable > quantity;
    }

    /// <summary>
    /// Sets the quantity available in inventory to 0.
    /// </summary>
    public void Reset()
    {
        QuantityAvailable = 0;
    }
}

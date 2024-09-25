using Domain.Common.Models;
using Domain.ProductAggregate.ValueObjects;

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
        : base(InventoryId.Create())
    {
        QuantityAvailable = quantityAvailable;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Inventory"/> class with the specified quantity available.
    /// </summary>
    /// <param name="quantityAvailable">The available quantity of the related product.</param>
    /// <returns>A new instance of <see cref="Inventory"/>.</returns>
    public static Inventory Create(int quantityAvailable)
    {
        return new Inventory(quantityAvailable);
    }
}
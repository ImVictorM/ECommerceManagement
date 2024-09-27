using Domain.Common.Models;

namespace Domain.ProductAggregate.ValueObjects;

/// <summary>
/// Represents an identifier for the <see cref="Entities.Inventory"/> entity.
/// </summary>
public sealed class InventoryId : ValueObject
{
    /// <summary>
    /// Gets the value of the identifier.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryId"/> class.
    /// </summary>
    private InventoryId() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryId"/> class with the specified identifier value.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private InventoryId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="InventoryId"/> class with a default identifier.
    /// </summary>
    /// <returns>A new instance with the default placeholder value of 0.</returns>
    public static InventoryId Create()
    {
        return new InventoryId(0);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="InventoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="InventoryId"/> class with the specified identifier.</returns>
    public static InventoryId Create(long value)
    {
        return new InventoryId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

using SharedKernel.Models;

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

    private InventoryId() { }

    private InventoryId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="InventoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>
    /// A new instance of the <see cref="InventoryId"/> class with the specified
    /// identifier.
    /// </returns>
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

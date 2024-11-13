using SharedKernel.Models;

namespace Domain.ShipmentAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Shipment"/> aggregate root.
/// </summary>
public sealed class ShipmentId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentId"/> class.
    /// </summary>
    private ShipmentId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ShipmentId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ShipmentId"/> class with the specified identifier.</returns>
    public static ShipmentId Create(long value)
    {
        return new ShipmentId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

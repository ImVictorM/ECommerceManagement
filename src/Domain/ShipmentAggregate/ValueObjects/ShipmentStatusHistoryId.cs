using Domain.Common.Models;

namespace Domain.ShipmentAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Entities.ShipmentStatusHistory"/>
/// </summary>
public sealed class ShipmentStatusHistoryId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistoryId"/> class.
    /// </summary>
    private ShipmentStatusHistoryId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ShipmentStatusHistoryId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatusHistoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatusHistoryId"/> class with the specified identifier.</returns>
    public static ShipmentStatusHistoryId Create(long value)
    {
        return new ShipmentStatusHistoryId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

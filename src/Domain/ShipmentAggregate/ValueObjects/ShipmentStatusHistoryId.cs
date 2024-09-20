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
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistoryId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ShipmentStatusHistoryId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatusHistoryId"/> class with id placeholder of 0.
    /// </summary>
    /// <returns>A new instance of the <see cref="ShipmentStatusHistoryId"/> class.</returns>
    public static ShipmentStatusHistoryId Create()
    {
        return new ShipmentStatusHistoryId(0);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

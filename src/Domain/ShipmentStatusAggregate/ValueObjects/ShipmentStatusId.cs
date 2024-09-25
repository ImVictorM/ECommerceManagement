using Domain.Common.Models;

namespace Domain.ShipmentStatusAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="ShipmentStatus"/> aggregate.
/// </summary>
public sealed class ShipmentStatusId : ValueObject
{
    /// <summary>
    /// Gets the identifier value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusId"/> class.
    /// </summary>
    private ShipmentStatusId() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    private ShipmentStatusId(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatusId"/> class with id placeholder of 0.
    /// </summary>
    /// <returns>A new instance of the <see cref="ShipmentStatusId"/> class.</returns>
    public static ShipmentStatusId Create()
    {
        return new ShipmentStatusId(0);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatusId"/> class.
    /// </summary>
    /// <param name="value">The identifier value.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatusId"/> class with the specified identifier.</returns>
    public static ShipmentStatusId Create(long value)
    {
        return new ShipmentStatusId(value);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

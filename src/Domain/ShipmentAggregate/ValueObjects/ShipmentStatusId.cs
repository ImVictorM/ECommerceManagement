using Domain.Common.Models;

namespace Domain.ShipmentAggregate.ValueObjects;

/// <summary>
/// Represents the identifier for the <see cref="Entities.ShipmentStatus"/> entity.
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

    /// <inheritdoc/>
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

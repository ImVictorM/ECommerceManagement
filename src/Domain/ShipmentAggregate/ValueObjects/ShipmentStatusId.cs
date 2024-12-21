using SharedKernel.Models;

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

    private ShipmentStatusId() { }

    private ShipmentStatusId(long value)
    {
        Value = value;
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

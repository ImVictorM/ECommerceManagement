using Domain.Common.Models;

namespace Domain.ShipmentAggregate.ValueObjects;

/// <summary>
/// Holds an historic change of status of a shipment.
/// </summary>
public sealed class ShipmentStatusHistory : ValueObject
{
    /// <summary>
    /// Gets the shipment status.
    /// </summary>
    public ShipmentStatus ShipmentStatus { get; } = null!;

    /// <summary>
    /// Gets the date the shipment status was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    private ShipmentStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    /// <param name="shipmentStatus">The shipment status.</param>
    private ShipmentStatusHistory(ShipmentStatus shipmentStatus)
    {
        ShipmentStatus = shipmentStatus;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    /// <param name="shipmentStatus">The shipment status.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatusHistory"/> class.</returns>
    public static ShipmentStatusHistory Create(ShipmentStatus shipmentStatus)
    {
        return new ShipmentStatusHistory(shipmentStatus);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ShipmentStatus;
        yield return CreatedAt;
    }
}

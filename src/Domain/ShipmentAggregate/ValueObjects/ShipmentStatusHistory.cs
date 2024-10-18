using Domain.Common.Models;

namespace Domain.ShipmentAggregate.ValueObjects;

/// <summary>
/// Holds an historic change of status of a shipment.
/// </summary>
public sealed class ShipmentStatusHistory : ValueObject
{
    /// <summary>
    /// Gets the shipment status id.
    /// </summary>
    public ShipmentStatusId ShipmentStatusId { get; } = null!;

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
    /// <param name="shipmentStatusId">The shipment status id.</param>
    private ShipmentStatusHistory(ShipmentStatusId shipmentStatusId)
    {
        ShipmentStatusId = shipmentStatusId;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    /// <param name="shipmentStatusId">The shipment status id.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatusHistory"/> class.</returns>
    public static ShipmentStatusHistory Create(ShipmentStatusId shipmentStatusId)
    {
        return new ShipmentStatusHistory(shipmentStatusId);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ShipmentStatusId;
        yield return CreatedAt;
    }
}

using Domain.Common.Models;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.ShipmentStatusAggregate.ValueObjects;

namespace Domain.ShipmentAggregate.Entities;

/// <summary>
/// Holds an historic change of status of a shipment.
/// </summary>
public sealed class ShipmentStatusHistory : Entity<ShipmentStatusHistoryId>
{
    /// <summary>
    /// Gets the shipment status change history.
    /// </summary>
    public ShipmentStatusId ShipmentStatusId { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    private ShipmentStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    /// <param name="shipmentStatusId">The shipment statuses history.</param>
    private ShipmentStatusHistory(ShipmentStatusId shipmentStatusId) : base(ShipmentStatusHistoryId.Create())
    {
        ShipmentStatusId = shipmentStatusId;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    /// <param name="shipmentStatusId">The shipment statuses history.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatusHistory"/> class.</returns>
    public static ShipmentStatusHistory Create(ShipmentStatusId shipmentStatusId)
    {
        return new ShipmentStatusHistory(shipmentStatusId);
    }
}

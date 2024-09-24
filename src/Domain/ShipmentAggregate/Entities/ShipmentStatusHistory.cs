using Domain.Common.Models;
using Domain.ShipmentAggregate.ValueObjects;

namespace Domain.ShipmentAggregate.Entities;

/// <summary>
/// Holds an historic change of status of a shipment.
/// </summary>
public sealed class ShipmentStatusHistory : Entity<ShipmentStatusHistoryId>
{
    /// <summary>
    /// Gets the shipment status change history.
    /// </summary>
    public IEnumerable<ShipmentStatusId> ShipmentStatuses { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    private ShipmentStatusHistory() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    /// <param name="shipmentStatuses">The shipment statuses history.</param>
    private ShipmentStatusHistory(IEnumerable<ShipmentStatusId> shipmentStatuses) : base(ShipmentStatusHistoryId.Create())
    {
        ShipmentStatuses = shipmentStatuses;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatusHistory"/> class.
    /// </summary>
    /// <param name="shipmentStatuses">The shipment statuses history.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatusHistory"/> class.</returns>
    public static ShipmentStatusHistory Create(IEnumerable<ShipmentStatusId> shipmentStatuses)
    {
        return new ShipmentStatusHistory(shipmentStatuses);
    }
}

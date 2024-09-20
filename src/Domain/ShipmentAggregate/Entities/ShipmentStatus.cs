using Domain.Common.Models;
using Domain.ShipmentAggregate.ValueObjects;

namespace Domain.ShipmentAggregate.Entities;

/// <summary>
/// Holds the current status of a <see cref="Shipment"/>.
/// </summary>
public sealed class ShipmentStatus : Entity<ShipmentStatusId>
{
    /// <summary>
    /// Gets the shipment status.
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatus"/> class.
    /// </summary>
    /// <param name="status">The shipment status.</param>
    private ShipmentStatus(string status) : base(ShipmentStatusId.Create())
    {
        Status = status;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentStatus"/> class.
    /// </summary>
    /// <param name="status">The shipment status.</param>
    /// <returns>A new instance of the <see cref="ShipmentStatus"/> class.</returns>
    public static ShipmentStatus Create(string status)
    {
        return new ShipmentStatus(status);
    }
}

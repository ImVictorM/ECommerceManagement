using Domain.Common.Models;
using Domain.ShipmentStatusAggregate.ValueObjects;

namespace Domain.ShipmentStatusAggregate;

/// <summary>
/// Represents the shipment status.
/// </summary>
public sealed class ShipmentStatus : AggregateRoot<ShipmentStatusId>
{
    /// <summary>
    /// Gets the shipment status.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatus"/> class.
    /// </summary>
    private ShipmentStatus() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="ShipmentStatus"/> class.
    /// </summary>
    /// <param name="status">The shipment status.</param>
    private ShipmentStatus(string status)
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

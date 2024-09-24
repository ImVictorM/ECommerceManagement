using Domain.Common.Models;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Entities;
using Domain.ShipmentAggregate.ValueObjects;

namespace Domain.ShipmentAggregate;

/// <summary>
/// Represents a shipment.
/// </summary>
public sealed class Shipment : AggregateRoot<ShipmentId>
{
    /// <summary>
    /// Gets the accountable for the delivery proccess.
    /// </summary>
    public string Accountable { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the order id this shipments is related.
    /// </summary>
    public OrderId OrderId { get; private set; } = null!;
    /// <summary>
    /// Get the shipment status.
    /// </summary>
    public ShipmentStatus ShipmentStatus { get; private set; } = null!;
    /// <summary>
    /// Gets the shipment status change history.
    /// </summary>
    public ShipmentStatusHistory ShipmentStatusHistory { get; private set; } = null!;

    /// <summary>
    /// Initiates a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    private Shipment() { }

    /// <summary>
    /// Initiates a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="orderId">The order id this shipment is related.</param>
    /// <param name="accountable">The accountable of the shipment.</param>
    /// <param name="shipmentStatus">The shipment status.</param>
    /// <param name="shipmentStatusHistory">The shipment status change history.</param>
    private Shipment(
        OrderId orderId,
        string accountable,
        ShipmentStatus shipmentStatus,
        ShipmentStatusHistory shipmentStatusHistory
    ) : base(ShipmentId.Create())
    {
        OrderId = orderId;
        Accountable = accountable;
        ShipmentStatus = shipmentStatus;
        ShipmentStatusHistory = shipmentStatusHistory;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="orderId">The order id this shipment is related.</param>
    /// <param name="accountable">The accountable of the shipment.</param>
    /// <param name="shipmentStatus">The shipment status.</param>
    /// <param name="shipmentStatusHistory">The shipment status change history.</param>
    /// <returns>A new instance of the <see cref="Shipment"/> class.</returns>
    public static Shipment Create(
        OrderId orderId,
        string accountable,
        ShipmentStatus shipmentStatus,
        ShipmentStatusHistory shipmentStatusHistory
    )
    {
        return new Shipment(
            orderId,
            accountable,
            shipmentStatus,
            shipmentStatusHistory
        );
    }
}

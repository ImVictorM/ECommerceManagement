using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.ShipmentAggregate;

/// <summary>
/// Represents a shipment.
/// </summary>
public sealed class Shipment : AggregateRoot<ShipmentId>
{
    /// <summary>
    /// The shipment status change history.
    /// </summary>
    private readonly List<ShipmentStatusHistory> _shipmentStatusHistories = [];
    /// <summary>
    /// Gets the accountable for the delivery process.
    /// </summary>
    public string Accountable { get; private set; } = string.Empty;
    /// <summary>
    /// Gets the order id this shipments is related.
    /// </summary>
    public OrderId OrderId { get; private set; } = null!;
    /// <summary>
    /// Get the shipment status identifier.
    /// </summary>
    public long ShipmentStatusId { get; private set; }
    /// <summary>
    /// Gets the shipment status change history.
    /// </summary>
    public IReadOnlyList<ShipmentStatusHistory> ShipmentStatusHistories => _shipmentStatusHistories.AsReadOnly();

    private Shipment() { }

    private Shipment(
        OrderId orderId,
        string accountable
    )
    {
        OrderId = orderId;
        Accountable = accountable;

        UpdateShipmentStatus(ShipmentStatus.Pending);
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="orderId">The order id this shipment is related.</param>
    /// <param name="accountable">The accountable of the shipment.</param>
    /// <returns>A new instance of the <see cref="Shipment"/> class.</returns>
    public static Shipment Create(
        OrderId orderId,
        string accountable
    )
    {
        return new Shipment(
            orderId,
            accountable
        );
    }

    private void UpdateShipmentStatus(ShipmentStatus status)
    {
        ShipmentStatusId = status.Id;
        _shipmentStatusHistories.Add(ShipmentStatusHistory.Create(ShipmentStatusId));
    }
}

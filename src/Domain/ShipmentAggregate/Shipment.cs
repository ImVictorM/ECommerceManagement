using Domain.CarrierAggregate.ValueObjects;
using Domain.OrderAggregate.ValueObjects;
using Domain.ShipmentAggregate.Enumerations;
using Domain.ShipmentAggregate.ValueObjects;
using Domain.ShippingMethodAggregate.ValueObjects;

using SharedKernel.Models;

namespace Domain.ShipmentAggregate;

/// <summary>
/// Represents a shipment.
/// </summary>
public sealed class Shipment : AggregateRoot<ShipmentId>
{
    private readonly List<ShipmentTrackingEntry> _shipmentTrackingEntries = [];
    private long _shipmentStatusId;

    /// <summary>
    /// Gets the shipment order id.
    /// </summary>
    public OrderId OrderId { get; private set; } = null!;
    /// <summary>
    /// Gets the shipment carrier id.
    /// </summary>
    public CarrierId CarrierId { get; private set; } = null!;
    /// <summary>
    /// Gets the shipping method id.
    /// </summary>
    public ShippingMethodId ShippingMethodId { get; private set; } = null!;
    /// <summary>
    /// Get the shipment status.
    /// </summary>
    public ShipmentStatus ShipmentStatus
    {
        get => BaseEnumeration.FromValue<ShipmentStatus>(_shipmentStatusId);
        private set => _shipmentStatusId = value.Id;
    }
    /// <summary>
    /// Gets the shipment tracking entries.
    /// </summary>
    public IReadOnlyList<ShipmentTrackingEntry> ShipmentTrackingEntries => _shipmentTrackingEntries.AsReadOnly();

    private Shipment() { }

    private Shipment(
        OrderId orderId,
        CarrierId carrierId,
        ShippingMethodId shippingMethodId
    )
    {
        OrderId = orderId;
        CarrierId = carrierId;
        ShippingMethodId = shippingMethodId;

        UpdateShipmentStatus(ShipmentStatus.First());
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Shipment"/> class.
    /// </summary>
    /// <param name="orderId">The shipment order id.</param>
    /// <param name="carrierId">The shipment carrier id.</param>
    /// <param name="shippingMethodId">The shipping method.</param>
    /// <returns>A new instance of the <see cref="Shipment"/> class.</returns>
    public static Shipment Create(
        OrderId orderId,
        CarrierId carrierId,
        ShippingMethodId shippingMethodId
    )
    {
        return new Shipment(
            orderId,
            carrierId,
            shippingMethodId
        );
    }

    /// <summary>
    /// Updates the current shipment status to the next.
    /// </summary>
    public void AdvanceShipmentStatus()
    {
        var nextStatus = ShipmentStatus.Advance();

        UpdateShipmentStatus(nextStatus);
    }

    private void UpdateShipmentStatus(ShipmentStatus status)
    {
        ShipmentStatus = status;
        _shipmentTrackingEntries.Add(ShipmentTrackingEntry.Create(ShipmentStatus));
    }
}

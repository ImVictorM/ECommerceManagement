using Domain.ShipmentAggregate.Enumerations;

using SharedKernel.Models;

namespace Domain.ShipmentAggregate.ValueObjects;

/// <summary>
/// Represents a shipment tracking entry.
/// </summary>
public sealed class ShipmentTrackingEntry : ValueObject
{
    private readonly long _shipmentStatusId;

    /// <summary>
    /// Gets the shipment status.
    /// </summary>
    public ShipmentStatus ShipmentStatus
    {
        get => BaseEnumeration.FromValue<ShipmentStatus>(_shipmentStatusId);
    }

    /// <summary>
    /// Gets the date the shipment tracking entry was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }

    private ShipmentTrackingEntry() { }

    private ShipmentTrackingEntry(ShipmentStatus shipmentStatus)
    {
        _shipmentStatusId = shipmentStatus.Id;

        CreatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="ShipmentTrackingEntry"/> class.
    /// </summary>
    /// <param name="shipmentStatus">The shipment status.</param>
    /// <returns>
    /// A new instance of the <see cref="ShipmentTrackingEntry"/> class.
    /// </returns>
    public static ShipmentTrackingEntry Create(ShipmentStatus shipmentStatus)
    {
        return new ShipmentTrackingEntry(shipmentStatus);
    }

    /// <inheritdoc/>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ShipmentStatus;
        yield return CreatedAt;
    }
}

using SharedKernel.Interfaces;

namespace Domain.ShipmentAggregate.Events;

/// <summary>
/// Represents an event that occurs after a shipment is delivered.
/// </summary>
/// <param name="Shipment">The delivered shipment.</param>
public record ShipmentDelivered(Shipment Shipment) : IDomainEvent;

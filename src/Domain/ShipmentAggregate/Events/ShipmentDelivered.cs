using SharedKernel.Interfaces;

namespace Domain.ShipmentAggregate.Events;

/// <summary>
/// Event that occurs after a shipment is delivered.
/// </summary>
/// <param name="Shipment">The delivered shipment.</param>
public record ShipmentDelivered(Shipment Shipment) : IDomainEvent;

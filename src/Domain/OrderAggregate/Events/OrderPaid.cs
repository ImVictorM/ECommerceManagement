using SharedKernel.Interfaces;

namespace Domain.OrderAggregate.Events;

/// <summary>
/// Represents an event that occurs when an order is paid.
/// </summary>
/// <param name="Order">The paid order.</param>
public record OrderPaid(Order Order) : IDomainEvent;

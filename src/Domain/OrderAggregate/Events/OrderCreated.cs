using SharedKernel.Interfaces;

namespace Domain.OrderAggregate.Events;

/// <summary>
/// Event generated when an order is created.
/// </summary>
/// <param name="Order">The order.</param>
public record OrderCreated(Order Order) : IDomainEvent;

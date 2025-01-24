using SharedKernel.Interfaces;

namespace Domain.OrderAggregate.Events;

/// <summary>
/// Event that occurs when an order is paid.
/// </summary>
/// <param name="Order">The order.</param>
public record OrderPaid(Order Order) : IDomainEvent;

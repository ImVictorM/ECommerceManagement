using SharedKernel.Interfaces;

namespace Domain.OrderAggregate.Events;

/// <summary>
/// Event that occurs when an order is canceled.
/// </summary>
/// <param name="Order">The canceled order.</param>
public record OrderCanceled(Order Order) : IDomainEvent;

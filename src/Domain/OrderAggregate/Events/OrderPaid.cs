using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate.Events;

/// <summary>
/// Event that occurs when an order is paid.
/// </summary>
/// <param name="Order">The order.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
public record OrderPaid(Order Order, Address DeliveryAddress) : IDomainEvent;

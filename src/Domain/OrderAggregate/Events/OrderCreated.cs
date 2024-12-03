using SharedKernel.Interfaces;

namespace Domain.OrderAggregate.Events;

/// <summary>
/// Event generated when an order is created.
/// </summary>
/// <param name="Order">The order.</param>
/// <param name="PaymentMethod">The payment method.</param>
/// <param name="Installments">The installments.</param>
public record OrderCreated(
    Order Order,
    IPaymentMethod PaymentMethod,
    int? Installments = null
) : IDomainEvent;

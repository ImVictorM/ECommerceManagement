using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Domain.OrderAggregate.Events;

/// <summary>
/// Event generated when an order is created.
/// </summary>
/// <param name="Order">The order.</param>
/// <param name="PaymentMethod">The payment method.</param>
/// <param name="BillingAddress">The payment billing address.</param>
/// <param name="DeliveryAddress">The order delivery address.</param>
/// <param name="Installments">The installments.</param>
public record OrderCreated(
    Order Order,
    IPaymentMethod PaymentMethod,
    Address BillingAddress,
    Address DeliveryAddress,
    int? Installments = null
) : IDomainEvent;

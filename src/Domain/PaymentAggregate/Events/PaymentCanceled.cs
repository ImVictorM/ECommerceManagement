using SharedKernel.Interfaces;

namespace Domain.PaymentAggregate.Events;

/// <summary>
/// Event triggered when a payment is canceled.
/// </summary>
/// <param name="payment">The canceled payment.</param>
public record PaymentCanceled(Payment payment) : IDomainEvent;

using SharedKernel.Interfaces;

namespace Domain.PaymentAggregate.Events;

/// <summary>
/// Event triggered when the payment status changes.
/// </summary>
/// <param name="Payment">The payment.</param>
public record PaymentStatusChanged(Payment Payment) : IDomainEvent;

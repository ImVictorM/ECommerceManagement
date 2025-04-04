using SharedKernel.Interfaces;

namespace Domain.PaymentAggregate.Events;

/// <summary>
/// Represents an event that occurs when a payment is canceled.
/// </summary>
/// <param name="Payment">The canceled payment.</param>
public record PaymentCanceled(Payment Payment) : IDomainEvent;

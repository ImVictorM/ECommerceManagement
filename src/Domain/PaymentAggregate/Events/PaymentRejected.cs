using SharedKernel.Interfaces;

namespace Domain.PaymentAggregate.Events;

/// <summary>
/// Represents an event that occurs when the payment is rejected.
/// </summary>
/// <param name="Payment">The rejected payment.</param>
public record PaymentRejected(Payment Payment) : IDomainEvent;

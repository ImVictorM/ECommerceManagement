using SharedKernel.Interfaces;

namespace Domain.PaymentAggregate.Events;

/// <summary>
/// Represents an event that occurs when a payment is approved.
/// </summary>
/// <param name="Payment">The approved payment.</param>
public record PaymentApproved(Payment Payment) : IDomainEvent;

using SharedKernel.Interfaces;

namespace Domain.PaymentAggregate.Events;

/// <summary>
/// Event that occurs when a payment is authorized.
/// </summary>
/// <param name="Payment">The authorized payment.</param>
public record PaymentAuthorized(Payment Payment) : IDomainEvent;

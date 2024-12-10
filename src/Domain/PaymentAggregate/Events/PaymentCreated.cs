using Domain.UserAggregate.ValueObjects;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Domain.PaymentAggregate.Events;

/// <summary>
/// Event triggered after an payment was created.
/// </summary>
/// <param name="Payment">The payment created.</param>
/// <param name="PayerId">The payer id.</param>
/// <param name="BillingAddress">The billing address.</param>
/// <param name="DeliveryAddress">The delivery address.</param>
public record PaymentCreated(Payment Payment, UserId PayerId, Address BillingAddress, Address DeliveryAddress) : IDomainEvent;

using Application.Common.Interfaces.Payments;
using Domain.PaymentAggregate.Enumerations;
using SharedKernel.ValueObjects;

namespace Infrastructure.Payments.Common.DTOs;

/// <summary>
/// Represents a generic response for a payment.
/// </summary>
/// <param name="PaymentId">The payment id.</param>
/// <param name="PaymentMethod">The payment method.</param>
/// <param name="Amount">The payment amount.</param>
/// <param name="Installments">The payment installments.</param>
/// <param name="Status">The payment status.</param>
/// <param name="Details">The payment details.</param>
/// <param name="Captured">A value indicating if the payment was captured.</param>
/// <param name="DeliveryAddress">The delivery address.</param>
public record PaymentResponse(
    string PaymentId,
    string PaymentMethod,
    decimal Amount,
    int Installments,
    PaymentStatus Status,
    string Details,
    bool Captured,
    Address DeliveryAddress
) : IPaymentResponse;

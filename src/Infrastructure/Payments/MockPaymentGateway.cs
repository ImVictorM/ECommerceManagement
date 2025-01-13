using Application.Common.Interfaces.Payments;

using Domain.OrderAggregate;
using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;
using Domain.UserAggregate;

using Infrastructure.Payments.Common.DTOs;

using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;

namespace Infrastructure.Payments;

/// <summary>
/// Represents a simple mocked structure of a payment gateway.
/// </summary>
public class MockPaymentGateway : IPaymentGateway
{

    /// <inheritdoc/>
    public async Task<IPaymentResponse> AuthorizePaymentAsync(
        Guid requestId,
        Order order,
        IPaymentMethod paymentMethod,
        User? payer = null,
        Address? billingAddress = null,
        int? installments = null
    )
    {
        await Task.CompletedTask;

        return new PaymentResponse(
            PaymentId: Guid.NewGuid().ToString(),
            PaymentMethod: "credit_card",
            Amount: 120m,
            Installments: 1,
            Status: PaymentStatus.FromDisplayName("pending"),
            Details: "does not matter",
            Captured: true
        );
    }

    /// <inheritdoc/>
    public async Task<IPaymentStatusResponse> CapturePaymentAsync(PaymentId paymentId)
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse(
            Status: PaymentStatus.FromDisplayName("approved"),
            Details: "accredited",
            Captured: true
        );
    }

    /// <inheritdoc/>
    public async Task<IPaymentStatusResponse> CancelAuthorizationAsync(PaymentId paymentId)
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse(
            Status: PaymentStatus.FromDisplayName("canceled"),
            Details: "by collector",
            Captured: false
        );
    }

    /// <inheritdoc/>
    public async Task<IPaymentRefundResponse> RefundPaymentAsync(PaymentId paymentId, decimal amount)
    {
        await Task.CompletedTask;

        return new PaymentRefundResponse(
            RefundId: Guid.NewGuid().ToString(),
            PaymentId: paymentId.ToString(),
            Amount: amount,
            Status: PaymentStatus.FromDisplayName("approved"),
            Reason: "does not matter",
            RefundMode: "standard"
        );
    }

    /// <inheritdoc/>
    public async Task<IPaymentResponse> GetPaymentByIdAsync(PaymentId paymentId)
    {
        await Task.CompletedTask;

        return new PaymentResponse(
            PaymentId: paymentId.ToString(),
            PaymentMethod: "credit_card",
            Amount: 120m,
            Installments: 1,
            Status: PaymentStatus.FromDisplayName("pending"),
            Details: "does not matter",
            Captured: true
        );
    }
}

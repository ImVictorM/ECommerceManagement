using Application.Common.Interfaces.Payments;
using Application.Common.Interfaces.Services;
using Domain.PaymentAggregate;
using Domain.PaymentAggregate.ValueObjects;
using Infrastructure.Payments.Common.DTOs;

namespace Infrastructure.Payments;

/// <summary>
/// Represents a simple mocked structure of a payment gateway.
/// </summary>
public class MockPaymentGateway : IPaymentGateway
{
    /// <inheritdoc/>
    public async Task<IAuthorizePaymentResponse> AuthorizePaymentAsync(Payment payment)
    {
        await Task.CompletedTask;

        return new AuthorizePaymentResponse(
            payment.Id.ToString(),
            "authorized",
            "pending capture",
            false
        );
    }

    /// <inheritdoc/>
    public async Task<IPaymentStatusResponse> CapturePaymentAsync(PaymentId paymentId)
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse("approved", "accredited", true);
    }

    /// <inheritdoc/>
    public async Task<IPaymentStatusResponse> CancelAuthorizationAsync(PaymentId paymentId)
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse("cancelled", "by collector", false);
    }

    /// <inheritdoc/>
    public async Task<IPaymentRefundResponse> RefundPaymentAsync(PaymentId paymentId, decimal amount)
    {
        await Task.CompletedTask;

        return new PaymentRefundResponse(
            Guid.NewGuid().ToString(),
            paymentId.ToString(),
            amount,
            "approved",
            "does not matter",
            "standard"
        );
    }
}

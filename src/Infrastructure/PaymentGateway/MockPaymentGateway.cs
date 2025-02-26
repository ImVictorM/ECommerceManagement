using Domain.PaymentAggregate.Enumerations;

using Application.Common.PaymentGateway;

using SharedKernel.Models;

namespace Infrastructure.PaymentGateway;

internal sealed class MockPaymentGateway : IPaymentGateway
{

    /// <inheritdoc/>
    public async Task<PaymentResponse> AuthorizePaymentAsync(AuthorizePaymentInput input)
    {
        await Task.CompletedTask;

        return new PaymentResponse(
            PaymentId: Guid.NewGuid().ToString(),
            PaymentMethod: "credit_card",
            Amount: 120m,
            Installments: 1,
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Pending"),
            Details: "does not matter",
            Captured: true
        );
    }

    /// <inheritdoc/>
    public async Task<PaymentStatusResponse> CapturePaymentAsync(string paymentId)
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse(
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Approved"),
            Details: "accredited",
            Captured: true
        );
    }

    /// <inheritdoc/>
    public async Task<PaymentStatusResponse> CancelAuthorizationAsync(string paymentId)
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse(
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Canceled"),
            Details: "by collector",
            Captured: false
        );
    }

    /// <inheritdoc/>
    public async Task<PaymentRefundResponse> RefundPaymentAsync(string paymentId, decimal amount)
    {
        await Task.CompletedTask;

        return new PaymentRefundResponse(
            RefundId: Guid.NewGuid().ToString(),
            PaymentId: paymentId,
            Amount: amount,
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Approved"),
            Reason: "does not matter",
            RefundMode: "standard"
        );
    }

    /// <inheritdoc/>
    public async Task<PaymentResponse> GetPaymentByIdAsync(string paymentId)
    {
        await Task.CompletedTask;

        return new PaymentResponse(
            PaymentId: paymentId,
            PaymentMethod: "credit_card",
            Amount: 120m,
            Installments: 1,
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Pending"),
            Details: "does not matter",
            Captured: true
        );
    }
}

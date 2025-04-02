using Application.Common.PaymentGateway.Requests;
using Application.Common.PaymentGateway.Responses;
using Application.Common.PaymentGateway.PaymentMethods;
using Application.Common.PaymentGateway;

using Domain.PaymentAggregate.Enumerations;
using Domain.PaymentAggregate.ValueObjects;

using SharedKernel.Models;

namespace Infrastructure.PaymentGateway;

internal sealed class MockPaymentGateway : IPaymentGateway
{
    public async Task<PaymentResponse> AuthorizePaymentAsync(
        AuthorizePaymentRequest request
    )
    {
        await Task.CompletedTask;

        return new PaymentResponse(
            PaymentId: PaymentId.Create(Guid.NewGuid().ToString()),
            PaymentMethod: new CreditCard("tokenized-card-data"),
            Amount: 120m,
            Installments: 1,
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Pending"),
            Details: "does not matter",
            Captured: true
        );
    }

    public async Task<PaymentStatusResponse> CapturePaymentAsync(
        PaymentId paymentId
    )
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse(
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Approved"),
            Details: "accredited",
            Captured: true
        );
    }

    public async Task<PaymentStatusResponse> CancelAuthorizationAsync(
        PaymentId paymentId
    )
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse(
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Canceled"),
            Details: "by collector",
            Captured: false
        );
    }

    public async Task<PaymentRefundResponse> RefundPaymentAsync(
        PaymentId paymentId,
        decimal amount
    )
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

    public async Task<PaymentResponse> GetPaymentByIdAsync(
        PaymentId paymentId,
        CancellationToken cancellationToken = default
    )
    {
        await Task.CompletedTask;

        return new PaymentResponse(
            PaymentId: paymentId,
            PaymentMethod:  new CreditCard("tokenized-card-data"),
            Amount: 120m,
            Installments: 1,
            Status: BaseEnumeration.FromDisplayName<PaymentStatus>("Pending"),
            Details: "does not matter",
            Captured: true
        );
    }
}

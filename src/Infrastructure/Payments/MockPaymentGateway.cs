using Application.Common.Interfaces.Payments;
using Domain.OrderAggregate;
using Domain.OrderAggregate.ValueObjects;
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
    public Task AuthorizePaymentAsync(
        Order order,
        IPaymentMethod paymentMethod,
        User? payer = null,
        Address? billingAddress = null,
        Address? deliveryAddress = null,
        int? installments = null
    )
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task<IPaymentStatusResponse> CapturePaymentAsync(OrderPaymentId paymentId)
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse("approved", "accredited", true);
    }

    /// <inheritdoc/>
    public async Task<IPaymentStatusResponse> CancelAuthorizationAsync(OrderPaymentId paymentId)
    {
        await Task.CompletedTask;

        return new PaymentStatusResponse("cancelled", "by collector", false);
    }

    /// <inheritdoc/>
    public async Task<IPaymentRefundResponse> RefundPaymentAsync(OrderPaymentId paymentId, decimal amount)
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

    /// <inheritdoc/>
    public async Task<IPaymentResponse> GetPaymentByIdAsync(OrderPaymentId paymentId)
    {
        await Task.CompletedTask;

        return new PaymentResponse(
            Guid.NewGuid().ToString(),
            "credit_card",
            120m,
            1,
            "Pending",
            "does not matter",
            true
        );
    }
}

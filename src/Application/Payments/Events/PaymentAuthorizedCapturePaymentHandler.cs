using Application.Common.PaymentGateway;
using Application.Common.Persistence;
using Domain.PaymentAggregate.Events;

using MediatR;

namespace Application.Payments.Events;

/// <summary>
/// Handles the <see cref="PaymentAuthorized"/> event by
/// capturing the payment.
/// </summary>
public sealed class PaymentAuthorizedCapturePaymentHandler : INotificationHandler<PaymentAuthorized>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initiates a new instance of the <see cref="PaymentAuthorizedCapturePaymentHandler"/> class.
    /// </summary>
    /// <param name="paymentGateway">The payment gateway.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public PaymentAuthorizedCapturePaymentHandler(IUnitOfWork unitOfWork, IPaymentGateway paymentGateway)
    {
        _paymentGateway = paymentGateway;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc/>
    public async Task Handle(PaymentAuthorized notification, CancellationToken cancellationToken)
    {
        var payment = notification.Payment;

        var captureResponse = await _paymentGateway.CapturePaymentAsync(payment.Id.ToString());

        payment.UpdatePaymentStatus(captureResponse.Status);

        await _unitOfWork.SaveChangesAsync();
    }
}
